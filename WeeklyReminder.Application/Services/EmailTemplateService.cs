using WeeklyReminder.Application.Repositories;
using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Services.Abstracts;

namespace WeeklyReminder.Application.Services;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IUnitOfWork unitOfWork)
    {
        _emailTemplateRepository = emailTemplateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmailTemplateEntity> GetByIdAsync(Guid id)
    {
        return await _emailTemplateRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<EmailTemplateEntity>> GetByActivityIdAsync(Guid activityId)
    {
        return await _emailTemplateRepository.GetByActivityIdAsync(activityId);
    }

    public async Task CreateEmailTemplateAsync(EmailTemplateEntity emailTemplate)
    {
        emailTemplate.Id = Guid.NewGuid();
        await _emailTemplateRepository.AddAsync(emailTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateEmailTemplateAsync(EmailTemplateEntity emailTemplate)
    {
        await _emailTemplateRepository.UpdateAsync(emailTemplate);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteEmailTemplateAsync(Guid id)
    {
        await _emailTemplateRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}