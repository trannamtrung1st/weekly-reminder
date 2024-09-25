using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.Domain.Models;

public class SettingsModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string AppPassword { get; set; }
}