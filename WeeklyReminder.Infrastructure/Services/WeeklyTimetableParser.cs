using ClosedXML.Excel;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Application.Services.Abstracts;

namespace WeeklyReminder.Infrastructure.Services;

public class WeeklyTimetableParser : IWeeklyTimetableParser
{
    public (ScheduleEntity Schedule, List<DayEntity> Days, List<TimeSlotEntity> TimeSlots, List<ActivityEntity> Activities) ParseTimetable(Stream excelStream)
    {
        using var workbook = new XLWorkbook(excelStream);
        var worksheet = workbook.Worksheets.First();

        var schedule = new ScheduleEntity
        {
            StartTime = TimeSpan.FromHours(7), // Default start time
            TimeInterval = TimeSpan.FromMinutes(15) // Default interval
        };

        var days = new List<DayEntity>();
        var timeSlots = new List<TimeSlotEntity>();
        var activities = new Dictionary<string, ActivityEntity>();

        for (int row = 5; row <= worksheet.LastRowUsed().RowNumber(); row++)
        {
            var time = TimeSpan.Parse(worksheet.Cell(row, 1).GetString());

            for (int col = 2; col <= 8; col++)
            {
                var cellValue = worksheet.Cell(row, col).GetString();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    var dayOfWeek = (DayOfWeek)(col - 2);
                    var day = days.FirstOrDefault(d => d.DayOfWeek == dayOfWeek) ?? new DayEntity { DayOfWeek = dayOfWeek };
                    if (!days.Contains(day)) days.Add(day);

                    if (!activities.TryGetValue(cellValue, out var activity))
                    {
                        activity = new ActivityEntity { Name = cellValue, Color = GenerateRandomColor() };
                        activities[cellValue] = activity;
                    }

                    var timeSlot = new TimeSlotEntity { StartTime = time, Activity = activity };
                    timeSlots.Add(timeSlot);
                    day.TimeSlots.Add(timeSlot);
                }
            }
        }

        return (schedule, days, timeSlots, activities.Values.ToList());
    }

    private string GenerateRandomColor()
    {
        var random = new Random();
        return $"#{random.Next(0x1000000):X6}";
    }
}