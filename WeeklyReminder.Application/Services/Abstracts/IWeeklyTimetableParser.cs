using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IWeeklyTimetableParser
{
    (Schedule Schedule, List<Day> Days, List<TimeSlot> TimeSlots, List<Activity> Activities) ParseTimetable(Stream excelStream);
}