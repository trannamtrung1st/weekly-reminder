namespace WeeklyReminder.Domain.Entities;

public class Day
{
    public Guid Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public List<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    public Guid ScheduleId { get; set; }
    public Schedule Schedule { get; set; }
}
