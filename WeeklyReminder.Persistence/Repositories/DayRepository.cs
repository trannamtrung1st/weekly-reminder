using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class DayRepository : IDayRepository
{
    private readonly WeeklyReminderDbContext _context;

    public DayRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<DayEntity> GetByIdAsync(Guid id)
    {
        return await _context.Days.FindAsync(id);
    }

    public async Task<IEnumerable<DayEntity>> GetAllAsync()
    {
        return await _context.Days.ToListAsync();
    }

    public async Task AddAsync(DayEntity day)
    {
        await _context.Days.AddAsync(day);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DayEntity day)
    {
        _context.Entry(day).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var day = await _context.Days.FindAsync(id);
        if (day != null)
        {
            _context.Days.Remove(day);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByScheduleIdAsync(Guid scheduleId)
    {
        var daysToDelete = await _context.Days
            .Where(d => d.ScheduleId == scheduleId)
            .ToListAsync();

        _context.Days.RemoveRange(daysToDelete);
        await _context.SaveChangesAsync();
    }
}
