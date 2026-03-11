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
    /// <param name="template">The email template to render.</param>
    /// <param name="parameters">The email template's parameters.</param>
    /// <param name="recipients">The email's recipients.</param>
    public static void Render(string subject, string template, Dictionary<string, object> parameters, params string[] recipients)
    {
        var config = Application.Current.Configuration;
        var message = CreateMessage(subject, config);

        message.IsBodyHtml = true;
        message.Body = File.ReadAllText(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "emails", $"{template}.html"));

        foreach (var parameter in parameters)
        {
            message.Body = Regex.Replace(
                input: message.Body, 
                pattern: @"\$\{\s*inputs\." + Regex.Escape(parameter.Key) + @"\s*\}", 
                replacement: parameter.Value?.ToString() ?? string.Empty);
        }

        Send(message, recipients);
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