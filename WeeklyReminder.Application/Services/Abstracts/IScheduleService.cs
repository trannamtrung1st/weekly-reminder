using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IScheduleService
{
    Task<ScheduleEntity> GetScheduleByIdAsync(Guid id);
    Task<IEnumerable<ScheduleEntity>> GetAllSchedulesAsync();
    Task CreateScheduleAsync(ScheduleEntity schedule);
    Task UpdateScheduleAsync(ScheduleEntity schedule);
    Task DeleteScheduleAsync(Guid id);
    Task CreateScheduleFromTimetableAsync(Guid userId, Stream timetableStream);
}
