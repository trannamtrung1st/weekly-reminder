using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;
using WeeklyReminder.Domain.Services;

namespace WeeklyReminder.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public UserService(IUnitOfWork unitOfWork, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
    }

    public async Task<UserEntity> GetUserByIdAsync(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<UserEntity> CreateUserAsync(UserEntity user)
    {
        // Check if username or email already exists
        var existingUser = await _userRepository.GetByUsernameOrEmailAsync(user.Username, user.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username or email already exists");
        }

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}