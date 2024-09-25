using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IWeeklyTimetableParser
{
    Task<(ScheduleEntity Schedule, List<TimeSlotEntity> TimeSlots, List<ActivityEntity> Activities)> ParseTimetable(Stream excelStream);
}