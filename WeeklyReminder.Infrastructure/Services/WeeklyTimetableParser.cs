using ClosedXML.Excel;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Application.Services.Abstracts;

namespace WeeklyReminder.Infrastructure.Services;

public class WeeklyTimetableParser : IWeeklyTimetableParser
{
    public async Task<(ScheduleEntity Schedule, List<TimeSlotEntity> TimeSlots, List<ActivityEntity> Activities)> ParseTimetable(Stream excelStream)
    {
        using var memStream = new MemoryStream();
        await excelStream.CopyToAsync(memStream);
        memStream.Position = 0;
        using var workbook = new XLWorkbook(memStream);
        var worksheet = workbook.Worksheets.First();

        var startTimeStr = worksheet.Cell("G3").GetFormattedString();
        var timeIntervalStr = worksheet.Cell("H3").GetString();
        var timeIntervalMins = int.Parse(timeIntervalStr.Split()[0]);
        var schedule = new ScheduleEntity
        {
            StartTime = DateTime.Parse(startTimeStr).TimeOfDay,
            TimeInterval = TimeSpan.FromMinutes(timeIntervalMins)
        };

        var timeSlots = new List<TimeSlotEntity>();
        var activities = new Dictionary<string, ActivityEntity>();

        // Parse time slots and activities
        for (int row = 6; row <= worksheet.LastRowUsed().RowNumber(); row++)
        {
            var timeStr = worksheet.Cell(row, 2).GetFormattedString();
            var time = DateTime.Parse(timeStr).TimeOfDay;

            for (int col = 3; col <= 10; col++)
            {
                var cellValue = worksheet.Cell(row, col).GetString();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    if (!activities.TryGetValue(cellValue, out var activity))
                    {
                        activity = new ActivityEntity { Name = cellValue };
                        activities[cellValue] = activity;
                    }

                    var dow = worksheet.Cell(5, col).GetString();
                    var timeSlot = new TimeSlotEntity
                    {
                        StartTime = time,
                        Activity = activity,
                        DoW = ParseDayOfWeek(dow)
                    };
                    timeSlots.Add(timeSlot);
                }
            }
        }

        return (schedule, timeSlots, activities.Values.ToList());
    }

    public static DayOfWeek ParseDayOfWeek(string day)
    {
        return day.ToUpper() switch
        {
            "MON" => DayOfWeek.Monday,
            "TUES" => DayOfWeek.Tuesday,
            "WED" => DayOfWeek.Wednesday,
            "THURS" => DayOfWeek.Thursday,
            "FRI" => DayOfWeek.Friday,
            "SAT" => DayOfWeek.Saturday,
            "SUN" => DayOfWeek.Sunday,
            _ => throw new ArgumentException($"Invalid day of week: {day}")
        };
    }
}