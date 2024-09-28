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
    private readonly IReminderRepository _reminderRepository;

    public ReminderService(
        IScheduleRepository scheduleRepository,
        IEmailTemplateService emailTemplateService,
        IEmailService emailService,
        ISettingsService settingsService,
        IReminderRepository reminderRepository)
    {
        _scheduleRepository = scheduleRepository;
        _emailTemplateService = emailTemplateService;
        _emailService = emailService;
        _settingsService = settingsService;
        _reminderRepository = reminderRepository;
    }

    public async Task CheckUpcomingActivitiesAndSendReminders()
    {
        var appSettings = _settingsService.GetAppSettings();
        var serverTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appSettings.ServerTimeZone);

        var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, serverTimeZone);
        var threshold = TimeSpan.FromMinutes(appSettings.ReminderThresholdMinutes);
        var upcomingTimeSlotEnd = now.Add(threshold);
        var upcomingTimeSlots = await _scheduleRepository.GetUpcomingTimeSlotsAsync(now, upcomingTimeSlotEnd);

        foreach (var timeSlot in upcomingTimeSlots)
        {
            var existingReminder = await _reminderRepository.GetMostRecentReminderForTimeSlotAsync(timeSlot.Id);
            if (existingReminder == null)
            {
                await SendReminderEmail(timeSlot.Schedule.User, timeSlot.Activity, timeSlot);
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
            var confirmationToken = GenerateConfirmationToken();
            var confirmationLink = GenerateConfirmationLink(confirmationToken);
            var body = randomTemplate.Body
                .Replace("{ActivityName}", activity.Name)
                .Replace("{StartTime}", timeSlot.StartTime.ToString("HH:mm"))
                .Replace("{ConfirmationLink}", confirmationLink);

            await _emailService.SendEmailAsync(user.Email, subject, body);

            var reminder = new ReminderEntity
            {
                TimeSlotId = timeSlot.Id,
                SentAt = DateTime.UtcNow,
                ConfirmationToken = confirmationToken
            };
            await _reminderRepository.AddAsync(reminder);
        }
    }

    private string GenerateConfirmationToken()
    {
        return Guid.NewGuid().ToString("N");
    }

    private string GenerateConfirmationLink(string token)
    {
        var appSettings = _settingsService.GetAppSettings();
        return $"{appSettings.ApplicationBaseUrl}/api/reminder/confirm/{token}";
    }

    public async Task<bool> ConfirmReminderAsync(string token)
    {
        var reminder = await _reminderRepository.GetByConfirmationTokenAsync(token);
        if (reminder == null || reminder.IsResolved)
        {
            return false;
        }

        reminder.IsResolved = true;
        await _reminderRepository.UpdateAsync(reminder);
        return true;
    }
}