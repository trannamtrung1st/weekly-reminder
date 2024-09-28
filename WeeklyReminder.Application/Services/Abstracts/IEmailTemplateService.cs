using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IEmailTemplateService
{
    Task<EmailTemplateEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<EmailTemplateEntity>> GetByActivityIdAsync(Guid activityId);
    Task CreateEmailTemplateAsync(EmailTemplateEntity emailTemplate);
    Task UpdateEmailTemplateAsync(EmailTemplateEntity emailTemplate);
    Task DeleteEmailTemplateAsync(Guid id);
}
