namespace WeeklyReminder.Domain.Entities;

public class TimeSlotEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public double StartTime { get; set; }
    public DayOfWeek DoW { get; set; }
    public Guid ActivityId { get; set; }
    public ActivityEntity Activity { get; set; }
    public Guid ScheduleId { get; set; }
    public ScheduleEntity Schedule { get; set; }
    public List<ReminderEntity> Reminders { get; set; } = new List<ReminderEntity>();
}
