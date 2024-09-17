using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IScheduleService
{
    Task<Schedule> GetScheduleByIdAsync(Guid id);
    Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
    Task CreateScheduleAsync(Schedule schedule);
    Task UpdateScheduleAsync(Schedule schedule);
    Task DeleteScheduleAsync(Guid id);
    Task CreateScheduleFromTimetableAsync(Guid userId, Stream timetableStream);
}
