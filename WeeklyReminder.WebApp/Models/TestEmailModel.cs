using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.WebApp.Models;

public class TestEmailModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Subject { get; set; } = "Test Email";

    [Required]
    public string Body { get; set; } = "This is a test email from WeeklyReminder.";
}