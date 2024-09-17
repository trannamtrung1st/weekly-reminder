using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface IUserRepository
{
    Task<UserEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<UserEntity>> GetAllAsync();
    Task AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task DeleteAsync(Guid id);
    Task<UserEntity> GetByUsernameOrEmailAsync(string username, string email);
}