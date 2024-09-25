using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly WeeklyReminderDbContext _context;

    public ScheduleRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<ScheduleEntity> GetByIdAsync(Guid id)
    {
        return await _context.Schedules.FindAsync(id);
    }

    public async Task<IEnumerable<ScheduleEntity>> GetAllAsync()
    {
        return await _context.Schedules.ToListAsync();
    }

    public async Task AddAsync(ScheduleEntity schedule)
    {
        await _context.Schedules.AddAsync(schedule);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ScheduleEntity schedule)
    {
        _context.Entry(schedule).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ScheduleEntity> GetByUserIdAsync(Guid userId)
    {
        return await _context.Schedules
            .Include(d => d.TimeSlots)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }
}
