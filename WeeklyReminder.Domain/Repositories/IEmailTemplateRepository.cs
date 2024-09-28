using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Repositories;

public interface IEmailTemplateRepository
{
    Task<EmailTemplateEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<EmailTemplateEntity>> GetByActivityIdAsync(Guid activityId);
    Task AddAsync(EmailTemplateEntity emailTemplate);
    Task UpdateAsync(EmailTemplateEntity emailTemplate);
    Task DeleteAsync(Guid id);
}