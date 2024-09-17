using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Persistence.Repositories;

public class TimeSlotRepository : ITimeSlotRepository
{
    private readonly WeeklyReminderDbContext _context;

    public TimeSlotRepository(WeeklyReminderDbContext context)
    {
        _context = context;
    }

    public async Task<TimeSlotEntity> GetByIdAsync(Guid id)
    {
        return await _context.TimeSlots.FindAsync(id);
    }

    public async Task<IEnumerable<TimeSlotEntity>> GetAllAsync()
    {
        return await _context.TimeSlots.ToListAsync();
    }

    public async Task AddAsync(TimeSlotEntity timeSlot)
    {
        await _context.TimeSlots.AddAsync(timeSlot);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimeSlotEntity timeSlot)
    {
        _context.Entry(timeSlot).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var timeSlot = await _context.TimeSlots.FindAsync(id);
        if (timeSlot != null)
        {
            _context.TimeSlots.Remove(timeSlot);
            await _context.SaveChangesAsync();
        }
    }
}
