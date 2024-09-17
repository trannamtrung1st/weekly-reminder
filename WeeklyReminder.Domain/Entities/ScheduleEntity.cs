namespace WeeklyReminder.Domain.Entities;

public class ScheduleEntity
{
    public Guid Id { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan TimeInterval { get; set; }
    public List<DayEntity> Days { get; set; } = new List<DayEntity>();
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }
}
