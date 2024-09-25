using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly WeeklyReminderDbContext _context;

    public UserRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(UserEntity user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task UpdateAsync(UserEntity user)
    {
        _context.Entry(user).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }

    public async Task<UserEntity> GetByUsernameOrEmailAsync(string username, string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
    }
}