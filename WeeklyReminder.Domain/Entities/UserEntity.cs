using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public List<ScheduleEntity> Schedules { get; set; } = new List<ScheduleEntity>();
}