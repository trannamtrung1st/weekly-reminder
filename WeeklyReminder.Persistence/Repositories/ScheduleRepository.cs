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
        return await _context.Schedules
            .Include(s => s.User)
            .Include(s => s.TimeSlots)
            .ThenInclude(t => t.Activity)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<ScheduleEntity>> GetAllAsync()
    {
        return await _context.Schedules
            .Include(s => s.User)
            .Include(s => s.TimeSlots)
                .ThenInclude(ts => ts.Activity)
            .ToListAsync();
    }

    public async Task AddAsync(ScheduleEntity schedule)
    {
        await _context.Schedules.AddAsync(schedule);
    }

    public async Task UpdateAsync(ScheduleEntity schedule)
    {
        if (_context.Entry(schedule).State == EntityState.Detached)
            _context.Entry(schedule).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
        }
    }

    public async Task<ScheduleEntity> GetByUserIdAsync(Guid userId)
    {
        return await _context.Schedules
            .Include(d => d.TimeSlots)
            .ThenInclude(d => d.Activity)
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task<List<TimeSlotEntity>> GetUpcomingTimeSlotsAsync(DateTime start, DateTime end)
    {
        var currentTime = start.TimeOfDay;
        var endTime = end.TimeOfDay;
        var currentDayOfWeek = start.DayOfWeek;

        return await _context.TimeSlots
            .Where(ts => ts.DoW == currentDayOfWeek &&
                         ts.StartTime >= currentTime.TotalMinutes &&
                         ts.StartTime <= endTime.TotalMinutes)
            .Include(ts => ts.Schedule)
                .ThenInclude(s => s.User)
            .Include(ts => ts.Activity)
            .ToListAsync();
    }
}
