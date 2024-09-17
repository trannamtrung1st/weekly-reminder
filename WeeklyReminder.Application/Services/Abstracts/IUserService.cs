using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IUserService
{
    Task<UserEntity> GetUserByIdAsync(Guid id);
    Task<IEnumerable<UserEntity>> GetAllUsersAsync();
    Task<UserEntity> CreateUserAsync(UserEntity user);
    Task UpdateUserAsync(UserEntity user);
    Task DeleteUserAsync(Guid id);
}