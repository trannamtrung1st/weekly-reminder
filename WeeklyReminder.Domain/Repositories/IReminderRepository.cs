using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IReminderRepository
{
    Task<ReminderEntity> GetByIdAsync(Guid id);
    Task<ReminderEntity> GetByConfirmationTokenAsync(string token);
    Task<ReminderEntity> GetMostRecentReminderForTimeSlotAsync(Guid timeSlotId);
    Task AddAsync(ReminderEntity reminder);
    Task UpdateAsync(ReminderEntity reminder);
    Task<int> GetUnresolvedCountForScheduleAsync(Guid scheduleId);
    Task DeleteAllForScheduleAsync(Guid scheduleId);
}