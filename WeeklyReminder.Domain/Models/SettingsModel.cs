using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.Domain.Models;

public class SettingsModel
{
    [Required]
    public string Username { get; set; }

    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string AppPassword { get; set; }
}