namespace WeeklyReminder.Application.Models;

public class AppSettings
{
    public string ApplicationBaseUrl { get; set; }
    public int ReminderThresholdMinutes { get; set; }
    public string ServerTimeZone { get; set; }
}