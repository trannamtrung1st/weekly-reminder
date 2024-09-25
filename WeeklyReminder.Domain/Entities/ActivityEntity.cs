namespace WeeklyReminder.Domain.Entities;

public class ActivityEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }

    public override string ToString() => Name;
}