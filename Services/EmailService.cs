using System.Net;
using System.Net.Mail;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailModel emailRequest)
    {
        var smtpSettings = _configuration.GetSection("EmailSettings");
        var host = smtpSettings["SmtpServer"];
        var port = smtpSettings.GetValue<int>("SmtpPort");
        var username = smtpSettings["UserName"];
        var password = smtpSettings["Password"];
        var fromEmail = smtpSettings["SenderEmail"] ?? "noreply@example.com";

        if (string.IsNullOrEmpty(host))
        {
            _logger.LogWarning("SMTP settings not configured. Email to {To} with subject {Subject} was not sent.", emailRequest.ToSomeone, emailRequest.Header);
            // For demonstration purposes, we'll just log it.
            return;
        }

        using var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = emailRequest.Header,
            Body = emailRequest.Content,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(emailRequest.ToSomeone);

        await client.SendMailAsync(mailMessage);
        _logger.LogInformation("Email sent to {To}", emailRequest.ToSomeone);
    }
}
