using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IActivityRepository
{
    Task<Activity> GetByIdAsync(Guid id);
    Task<IEnumerable<Activity>> GetAllAsync();
    Task AddAsync(Activity activity);
    Task UpdateAsync(Activity activity);
    Task DeleteAsync(Guid id);
    Task<Activity> GetOrCreateByNameAsync(string name);
}
