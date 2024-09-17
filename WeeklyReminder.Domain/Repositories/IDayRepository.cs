using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IDayRepository
{
    Task<Day> GetByIdAsync(Guid id);
    Task<IEnumerable<Day>> GetAllAsync();
    Task AddAsync(Day day);
    Task UpdateAsync(Day day);
    Task DeleteAsync(Guid id);
    Task DeleteByScheduleIdAsync(Guid scheduleId);
}
