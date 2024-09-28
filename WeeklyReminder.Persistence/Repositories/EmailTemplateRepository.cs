using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Application.Repositories;
using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Persistence.Repositories;

public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly WeeklyReminderDbContext _context;

    public EmailTemplateRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<EmailTemplateEntity> GetByIdAsync(Guid id)
    {
        return await _context.EmailTemplates.FindAsync(id);
    }

    public async Task<IEnumerable<EmailTemplateEntity>> GetByActivityIdAsync(Guid activityId)
    {
        return await _context.EmailTemplates
            .Include(et => et.Activity)
            .Where(et => et.ActivityId == activityId)
            .ToListAsync();
    }

    public async Task AddAsync(EmailTemplateEntity emailTemplate)
    {
        await _context.EmailTemplates.AddAsync(emailTemplate);
    }

    public async Task UpdateAsync(EmailTemplateEntity emailTemplate)
    {
        if (_context.Entry(emailTemplate).State == EntityState.Detached)
            _context.Entry(emailTemplate).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id)
    {
        var emailTemplate = await GetByIdAsync(id);
        if (emailTemplate != null)
        {
            _context.EmailTemplates.Remove(emailTemplate);
        }
    }
}