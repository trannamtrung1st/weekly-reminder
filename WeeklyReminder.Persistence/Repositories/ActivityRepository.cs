using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly WeeklyReminderDbContext _context;

    public ActivityRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityEntity> GetByIdAsync(Guid id)
    {
        return await _context.Activities.FindAsync(id);
    }

    public async Task<IEnumerable<ActivityEntity>> GetAllAsync()
    {
        return await _context.Activities.ToListAsync();
    }

    public async Task AddAsync(ActivityEntity activity)
    {
        await _context.Activities.AddAsync(activity);
    }

    public async Task UpdateAsync(ActivityEntity activity)
    {
        if (_context.Entry(activity).State == EntityState.Detached)
            _context.Entry(activity).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity != null)
        {
            _context.Activities.Remove(activity);
        }
    }

    public async Task<IEnumerable<ActivityEntity>> GetByScheduleIdAsync(Guid scheduleId)
    {
        return await _context.Activities
            .Where(a => a.ScheduleId == scheduleId)
            .ToListAsync();
    }
}
