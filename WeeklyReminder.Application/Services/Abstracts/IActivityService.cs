using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IActivityService
{
    Task<ActivityEntity> GetActivityByIdAsync(Guid id);
    Task<IEnumerable<ActivityEntity>> GetActivitiesByScheduleIdAsync(Guid scheduleId);
    Task<IEnumerable<ActivityEntity>> GetAllActivitiesAsync();
    Task<ActivityEntity> CreateActivityAsync(ActivityEntity activity);
    Task UpdateActivityAsync(ActivityEntity activity);
    Task DeleteActivityAsync(Guid id);
}