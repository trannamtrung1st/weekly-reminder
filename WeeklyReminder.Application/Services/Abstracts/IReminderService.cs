using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IReminderService
{
    Task CheckUpcomingActivitiesAndSendReminders();
    Task SendReminderEmail(UserEntity user, ActivityEntity activity, TimeSlotEntity timeSlot);
}