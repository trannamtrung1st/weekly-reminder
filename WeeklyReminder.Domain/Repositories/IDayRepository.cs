using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IDayRepository
{
    Task<DayEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<DayEntity>> GetAllAsync();
    Task AddAsync(DayEntity day);
    Task UpdateAsync(DayEntity day);
    Task DeleteAsync(Guid id);
    Task DeleteByScheduleIdAsync(Guid scheduleId);
}
