﻿using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AlphaIDPlatform.Platform;

/// <summary>
/// Email Sender.
/// </summary>
public class SmtpMailSender : IEmailSender
{
    private readonly SmtpMailSenderOptions options;
    private readonly ILogger<SmtpMailSender>? logger;

    /// <summary>
    /// Initialize Email sender via options and logger.
    /// </summary>
    /// <param name="options">Options to configure this mail sender.</param>
    /// <param name="logger">Logger for logging of this mail sender.</param>
    public SmtpMailSender(IOptions<SmtpMailSenderOptions> options, ILogger<SmtpMailSender>? logger)
    {
        this.options = options.Value;
        this.logger = logger;
    }

    /// <summary>
    /// Send email to specificed recipient with subject and html message body.
    /// </summary>
    /// <param name="email">Recipient.</param>
    /// <param name="subject">Title of this mail.</param>
    /// <param name="htmlMessage">HTML mail body.</param>
    /// <returns>Task self.</returns>
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var smtpClient = new SmtpClient(this.options.Server, this.options.Port);
        if (this.options.UseDefaultCredentials)
        {
            smtpClient.UseDefaultCredentials = true;
        }
        else
        {
            var cred = new NetworkCredential(this.options.UserName, this.options.Password);
            smtpClient.Credentials = cred;
        }

        var to = new MailAddress(email);
        var from = new MailAddress(this.options.FromMailAddress, this.options.FromDisplayName, Encoding.UTF8);
        var message = new MailMessage(from, to)
        {
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        try
        {
            await smtpClient.SendMailAsync(message);
            this.logger?.LogInformation("已向{email}发送了标题为'{subject}'的邮件。", to.Address, subject);
        }
        catch (Exception ex)
        {
            this.logger?.LogError("发送邮件时发生异常，消息是{message}", ex.Message);
            throw;
        }

    }
}
