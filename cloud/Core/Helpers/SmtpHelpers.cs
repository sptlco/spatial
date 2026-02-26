// Copyright © Spatial Corporation. All rights reserved.

using Polly;
using Spatial.Extensions;
using System.Net;
using System.Net.Mail;

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
        var client = new SmtpClient(config.SMTP.Host) {
            Port = config.SMTP.Port,
            Credentials = new NetworkCredential(config.SMTP.Username, config.SMTP.Password),
            EnableSsl = true
        };

        var message = new MailMessage()
        {
            From = new MailAddress(config.SMTP.Username, config.SMTP.Name),
            Subject = subject,
            Body = body
        };

        recipients.ForEach(recipient => message.To.Add(new MailAddress(recipient)));

        new ResiliencePipelineBuilder()
            .AddRetry(new Polly.Retry.RetryStrategyOptions())
            .Build()
            .Execute(() => client.Send(message));
    }
}