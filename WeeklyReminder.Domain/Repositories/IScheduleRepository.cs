using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IScheduleRepository
{
    Task<ScheduleEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<ScheduleEntity>> GetAllAsync();
    Task AddAsync(ScheduleEntity schedule);
    Task UpdateAsync(ScheduleEntity schedule);
    Task DeleteAsync(Guid id);
    Task<ScheduleEntity> GetByUserIdAsync(Guid userId);
}
