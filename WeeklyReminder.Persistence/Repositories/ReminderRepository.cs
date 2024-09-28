using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly WeeklyReminderDbContext _context;

    public ReminderRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<ReminderEntity> GetByIdAsync(Guid id)
    {
        return await _context.Reminders.FindAsync(id);
    }

    public async Task<ReminderEntity> GetByConfirmationTokenAsync(string token)
    {
        return await _context.Reminders.FirstOrDefaultAsync(r => r.ConfirmationToken == token);
    }

    public async Task<ReminderEntity> GetMostRecentReminderForTimeSlotAsync(Guid timeSlotId)
    {
        return await _context.Reminders
            .Where(r => r.TimeSlotId == timeSlotId)
            .OrderByDescending(r => r.SentAt)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(ReminderEntity reminder)
    {
        await _context.Reminders.AddAsync(reminder);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ReminderEntity reminder)
    {
        _context.Entry(reminder).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnresolvedCountForScheduleAsync(Guid scheduleId)
    {
        return await _context.Reminders
            .Where(r => r.TimeSlot.ScheduleId == scheduleId && !r.IsResolved)
            .CountAsync();
    }

    public async Task DeleteAllForScheduleAsync(Guid scheduleId)
    {
        var remindersToDelete = await _context.Reminders
            .Where(r => r.TimeSlot.ScheduleId == scheduleId)
            .ToListAsync();

        _context.Reminders.RemoveRange(remindersToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnresolvedCountForActivityAsync(Guid activityId)
    {
        return await _context.Reminders
            .Where(r => r.TimeSlot.ActivityId == activityId && !r.IsResolved)
            .CountAsync();
    }
}