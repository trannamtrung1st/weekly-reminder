using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IScheduleRepository
{
    Task<Schedule> GetByIdAsync(Guid id);
    Task<IEnumerable<Schedule>> GetAllAsync();
    Task AddAsync(Schedule schedule);
    Task UpdateAsync(Schedule schedule);
    Task DeleteAsync(Guid id);
    Task<Schedule> GetByUserIdAsync(Guid userId);
}
