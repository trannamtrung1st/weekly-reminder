using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IUserService
{
    Task<User> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
}