using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly WeeklyReminderDbContext _context;

    public ActivityRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<Activity> GetByIdAsync(Guid id)
    {
        return await _context.Activities.FindAsync(id);
    }

    public async Task<IEnumerable<Activity>> GetAllAsync()
    {
        return await _context.Activities.ToListAsync();
    }

    public async Task AddAsync(Activity activity)
    {
        await _context.Activities.AddAsync(activity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Activity activity)
    {
        _context.Entry(activity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity != null)
        {
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Activity> GetOrCreateByNameAsync(string name)
    {
        var activity = await _context.Activities.FirstOrDefaultAsync(a => a.Name == name);
        if (activity == null)
        {
            activity = new Activity { Name = name, Color = GenerateRandomColor() };
            await _context.Activities.AddAsync(activity);
        }
        return activity;
    }

    private string GenerateRandomColor()
    {
        var random = new Random();
        return $"#{random.Next(0x1000000):X6}";
    }
}
