using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Domain.Repositories;

public interface ITimeSlotRepository
{
    Task<TimeSlotEntity> GetByIdAsync(Guid id);
    Task<IEnumerable<TimeSlotEntity>> GetAllAsync();
    Task AddAsync(TimeSlotEntity timeSlot);
    Task UpdateAsync(TimeSlotEntity timeSlot);
    Task DeleteAsync(Guid id);
}
