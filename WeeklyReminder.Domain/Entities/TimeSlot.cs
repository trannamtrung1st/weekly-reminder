namespace WeeklyReminder.Domain.Entities;

public class TimeSlot
{
    public Guid Id { get; set; }
    public TimeSpan StartTime { get; set; }
    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; }
}
