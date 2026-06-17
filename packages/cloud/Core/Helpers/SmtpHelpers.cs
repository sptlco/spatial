// Copyright © Spatial Corporation. All rights reserved.

using Polly;
using Spatial.Extensions;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Spatial.Helpers;

/// <summary>
/// Helper methods for SMTP.
/// </summary>
public static class Smtp
{
    /// <summary>
    /// Send an email.
    /// </summary>
    /// <param name="subject">The email's subject.</param>
    /// <param name="body">The email's body.</param>
    /// <param name="recipients">The email's recipients.</param>
    public static void Send(string subject, string body, params string[] recipients)
    {
        var config = Application.Current.Configuration;
        var message = CreateMessage(subject, config);

        message.Body = body;

        Send(message, recipients);
    }

    /// <summary>
    /// Render an email template.
    /// </summary>
    /// <param name="subject">The email's subject.</param>
    /// <param name="preview">The email's preview text.</param>
    /// <param name="template">The email template to render.</param>
    /// <param name="parameters">The email template's parameters.</param>
    /// <param name="recipients">The email's recipients.</param>
    public static void Send(string subject, string preview, string template, Dictionary<string, object> parameters, params string[] recipients)
    {
        var config = Application.Current.Configuration;
        var message = CreateMessage(subject, config);

        parameters["preview"] = preview;

        message.IsBodyHtml = true;
        message.Body = Render(template, parameters);

        Send(message, recipients);
    }

    private static string Render(string template, Dictionary<string, object> parameters)
    {
        var text = File.ReadAllText(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "emails", $"{template}.mail"));
        var body = Resolve(text);

        foreach (var parameter in parameters)
        {
            body = Regex.Replace(
                input: body, 
                pattern: @"\$\{\s*parameters\." + Regex.Escape(parameter.Key) + @"\s*\}", 
                replacement: parameter.Value?.ToString() ?? string.Empty);
        }

        return body;
    }

    private static string Resolve(string body, HashSet<string>? visited = null)
    {
        visited ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        return Regex.Replace(
            input: body,
            pattern: @"\$\{\s*templates\.(\w+)\s*\}",
            evaluator: (match) => {
                var name = match.Groups[1].Value;

                if (!visited.Add(name))
                {
                    throw new InvalidOperationException($"Circular template reference detected: '{name}'.");
                }

                var path = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "emails", $"{name}.mail");

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Template '{name}' not found.", path);
                }

                var nested = Resolve(File.ReadAllText(path), visited);

                visited.Remove(name);

                return nested;
            });
    }

    private static void Send(MailMessage message, params string[] recipients)
    {
        var config = Application.Current.Configuration;
        var client = new SmtpClient(config.SMTP.Host) {
            Port = config.SMTP.Port,
            Credentials = new NetworkCredential(config.SMTP.Username, config.SMTP.Password),
            EnableSsl = true
        };

        recipients.ForEach(recipient => message.To.Add(new MailAddress(recipient)));

        new ResiliencePipelineBuilder()
            .AddRetry(new Polly.Retry.RetryStrategyOptions())
            .Build()
            .Execute(() => client.Send(message));
    }

    private static MailMessage CreateMessage(string subject, Configuration config)
    {
        return new MailMessage {
            From = new MailAddress(config.SMTP.Username, config.SMTP.Name),
            Subject = subject
        };
    }
}