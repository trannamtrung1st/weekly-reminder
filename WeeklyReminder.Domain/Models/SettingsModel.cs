using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.Domain.Models;

public class SettingsModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string AppPassword { get; set; }
    public string ServerTimeZone { get; set; }
}