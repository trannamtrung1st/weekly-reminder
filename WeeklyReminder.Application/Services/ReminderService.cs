using System.Collections.Concurrent;
using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;

namespace WeeklyReminder.Application.Services;

public class ReminderService : IReminderService
{
    public const string SubjectPrefix = "[WR]";

    private readonly IScheduleRepository _scheduleRepository;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IEmailService _emailService;
    private readonly ISettingsService _settingsService;
    private readonly ConcurrentDictionary<Guid, DateTime> _remindedTimeSlots = new();

    public ReminderService(
        IScheduleRepository scheduleRepository,
        IEmailTemplateService emailTemplateService,
        IEmailService emailService,
        ISettingsService settingsService)
    {
        _scheduleRepository = scheduleRepository;
        _emailTemplateService = emailTemplateService;
        _emailService = emailService;
        _settingsService = settingsService;
    }

    public async Task CheckUpcomingActivitiesAndSendReminders()
    {
        var settings = await _settingsService.GetSettingsAsync(getSecrets: false);
        var serverTimeZone = TimeZoneInfo.FindSystemTimeZoneById(settings.ServerTimeZone);

        var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, serverTimeZone);
        var threshold = TimeSpan.FromMinutes(5);
        var upcomingTimeSlotEnd = now.Add(threshold);

        CleanUpPastReminders(now);

        var upcomingTimeSlots = await _scheduleRepository.GetUpcomingTimeSlotsAsync(now, upcomingTimeSlotEnd);

        foreach (var timeSlot in upcomingTimeSlots)
        {
            if (!_remindedTimeSlots.ContainsKey(timeSlot.Id))
            {
                await SendReminderEmail(timeSlot.Schedule.User, timeSlot.Activity, timeSlot);
                _remindedTimeSlots.TryAdd(timeSlot.Id, now.Add(threshold));
            }
        }
    }

    public async Task SendReminderEmail(UserEntity user, ActivityEntity activity, TimeSlotEntity timeSlot)
    {
        var templates = await _emailTemplateService.GetByActivityIdAsync(activity.Id);
        if (templates.Any())
        {
            var randomTemplate = templates.OrderBy(_ => Guid.NewGuid()).First();
            var subject = $"{SubjectPrefix} {randomTemplate.Subject}";
            var body = randomTemplate.Body
                .Replace("{ActivityName}", activity.Name)
                .Replace("{StartTime}", timeSlot.StartTime.ToString("HH:mm"));

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
    }

    private void CleanUpPastReminders(DateTime now)
    {
        foreach (var kvp in _remindedTimeSlots)
        {
            if (kvp.Value < now)
            {
                _remindedTimeSlots.TryRemove(kvp.Key, out _);
            }
        }
    }
}