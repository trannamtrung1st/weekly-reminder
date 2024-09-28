using System.Net;
using System.Net.Mail;
using WeeklyReminder.Application.Services.Abstracts;

namespace WeeklyReminder.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ISettingsService _settingsService;

    public EmailService(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var credentials = await _settingsService.GetSystemCredentialsAsync();
        var fromEmail = credentials.Email;
        var appPassword = credentials.AppPassword;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, appPassword),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}