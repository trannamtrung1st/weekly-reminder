namespace WeeklyReminder.Domain.Entities;

public class TimeSlotEntity
{
    public Guid Id { get; set; }
    public TimeSpan StartTime { get; set; }
    public Guid ActivityId { get; set; }
    public ActivityEntity Activity { get; set; }
    public Guid DayId { get; set; }
    public DayEntity Day { get; set; }
}
