using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Name { get; set; }

    public List<Schedule> Schedules { get; set; } = new List<Schedule>();
}