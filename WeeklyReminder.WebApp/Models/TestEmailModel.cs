using System.ComponentModel.DataAnnotations;

namespace WeeklyReminder.WebApp.Models;

public class TestEmailModel
{
    [Required]
    [EmailAddress]
    public string TestEmail { get; set; }

    [Required]
    public string EmailTitle { get; set; } = "Test Email";

    [Required]
    public string EmailContent { get; set; } = "This is a test email from WeeklyReminder.";
}