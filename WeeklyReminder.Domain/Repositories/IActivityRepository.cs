using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IActivityRepository
{
    Task<ActivityEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<ActivityEntity>> GetAllAsync();
    Task AddAsync(ActivityEntity activity);
    Task UpdateAsync(ActivityEntity activity);
    Task DeleteAsync(Guid id);
    Task<ActivityEntity> GetOrCreateByNameAsync(string name);
}
