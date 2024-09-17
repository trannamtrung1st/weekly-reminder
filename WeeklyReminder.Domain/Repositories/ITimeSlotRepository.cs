using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface ITimeSlotRepository
{
    Task<TimeSlot> GetByIdAsync(Guid id);
    Task<IEnumerable<TimeSlot>> GetAllAsync();
    Task AddAsync(TimeSlot timeSlot);
    Task UpdateAsync(TimeSlot timeSlot);
    Task DeleteAsync(Guid id);
}
