namespace WeeklyReminder.Application.Services.Abstracts;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}