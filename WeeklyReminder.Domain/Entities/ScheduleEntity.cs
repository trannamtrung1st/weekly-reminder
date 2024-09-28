namespace WeeklyReminder.Domain.Entities;

public class ScheduleEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public TimeSpan StartTime { get; set; }
    public TimeSpan TimeInterval { get; set; }
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
    public List<ActivityEntity> Activities { get; set; } = [];
    public List<TimeSlotEntity> TimeSlots { get; set; } = [];
}