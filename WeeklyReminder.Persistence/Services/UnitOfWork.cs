using WeeklyReminder.Domain.Repositories;
using WeeklyReminder.Domain.Services.Abstracts;

namespace WeeklyReminder.Persistence.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly WeeklyReminderDbContext _context;

    public UnitOfWork(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}