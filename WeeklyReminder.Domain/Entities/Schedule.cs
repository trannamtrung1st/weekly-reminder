namespace WeeklyReminder.Domain.Entities;

public class Schedule
{
    public Guid Id { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan TimeInterval { get; set; }
    public List<Day> Days { get; set; } = new List<Day>();
    public Guid UserId { get; set; }
    public User User { get; set; }
}
