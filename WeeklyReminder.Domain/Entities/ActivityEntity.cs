namespace WeeklyReminder.Domain.Entities;

public class ActivityEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public Guid ScheduleId { get; set; }
    public ScheduleEntity Schedule { get; set; }
    public List<EmailTemplateEntity> EmailTemplates { get; set; } = [];
    public List<TimeSlotEntity> TimeSlots { get; set; } = [];

    public override string ToString() => Name;
}