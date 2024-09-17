using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface IWeeklyTimetableParser
{
    (ScheduleEntity Schedule, List<DayEntity> Days, List<TimeSlotEntity> TimeSlots, List<ActivityEntity> Activities) ParseTimetable(Stream excelStream);
}