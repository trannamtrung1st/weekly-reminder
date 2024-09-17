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

    public async Task<User> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
        }
    }
}