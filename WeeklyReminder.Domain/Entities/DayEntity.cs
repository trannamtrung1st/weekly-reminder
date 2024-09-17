namespace WeeklyReminder.Domain.Entities;

public class DayEntity
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public List<TimeSlotEntity> TimeSlots { get; set; } = new List<TimeSlotEntity>();
    public Guid ScheduleId { get; set; }
    public ScheduleEntity Schedule { get; set; }
}
