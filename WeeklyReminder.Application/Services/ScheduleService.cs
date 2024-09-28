using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;
using WeeklyReminder.Domain.Services.Abstracts;
using System.Collections.Generic;
using System.Linq;

namespace WeeklyReminder.Application.Services;

public class ScheduleService : IScheduleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly ITimeSlotRepository _timeSlotRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IWeeklyTimetableParser _timetableParser;

    public ScheduleService(
        IUnitOfWork unitOfWork,
        IScheduleRepository scheduleRepository,
        ITimeSlotRepository timeSlotRepository,
        IActivityRepository activityRepository,
        IWeeklyTimetableParser timetableParser)
    {
        _unitOfWork = unitOfWork;
        _scheduleRepository = scheduleRepository;
        _timeSlotRepository = timeSlotRepository;
        _activityRepository = activityRepository;
        _timetableParser = timetableParser;
    }

    public async Task<ScheduleEntity> GetScheduleByIdAsync(Guid id)
    {
        return await _scheduleRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ScheduleEntity>> GetAllSchedulesAsync()
    {
        return await _scheduleRepository.GetAllAsync();
    }

    public async Task CreateScheduleAsync(ScheduleEntity schedule)
    {
        await _scheduleRepository.AddAsync(schedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateScheduleAsync(ScheduleEntity schedule)
    {
        await _scheduleRepository.UpdateAsync(schedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteScheduleAsync(Guid id)
    {
        await _scheduleRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateScheduleFromTimetableAsync(Guid userId, Stream timetableStream)
    {
        var (schedule, timeSlots, activities) = await _timetableParser.ParseTimetable(timetableStream);
        schedule.UserId = userId;

        var existingSchedule = await _scheduleRepository.GetByUserIdAsync(userId);
        if (existingSchedule != null)
        {
            schedule.Id = existingSchedule.Id;
            await _scheduleRepository.UpdateAsync(schedule);
        }
        else
        {
            await _scheduleRepository.AddAsync(schedule);
        }

        var activityDict = new Dictionary<string, ActivityEntity>(StringComparer.OrdinalIgnoreCase);

        foreach (var activity in activities)
        {
            activity.ScheduleId = schedule.Id;
            await _activityRepository.AddAsync(activity);
            activityDict[activity.Name] = activity;
        }

        foreach (var timeSlot in timeSlots)
        {
            if (activityDict.TryGetValue(timeSlot.Activity.Name, out var activity))
            {
                timeSlot.ActivityId = activity.Id;
                timeSlot.Activity = activity;
                timeSlot.ScheduleId = schedule.Id;
                timeSlot.Schedule = schedule;
                await _timeSlotRepository.AddAsync(timeSlot);
            }
            else
            {
                // This shouldn't happen if the activities are properly synchronized
                throw new Exception($"Activity '{timeSlot.Activity.Name}' not found");
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateScheduleFromTimetableAsync(Guid scheduleId, Stream timetableStream)
    {
        var existingSchedule = await _scheduleRepository.GetByIdAsync(scheduleId);
        if (existingSchedule == null)
        {
            throw new Exception("Schedule not found");
        }

        var (updatedSchedule, timeSlots, activities) = await _timetableParser.ParseTimetable(timetableStream);

        // Update existing schedule properties
        existingSchedule.StartTime = updatedSchedule.StartTime;
        existingSchedule.TimeInterval = updatedSchedule.TimeInterval;

        // Clear existing time slots
        await _timeSlotRepository.DeleteByScheduleIdAsync(scheduleId);

        // Get existing activities
        var existingActivities = await _activityRepository.GetByScheduleIdAsync(scheduleId);
        var activityDict = existingActivities.ToDictionary(a => a.Name, StringComparer.OrdinalIgnoreCase);

        // Update or add activities
        foreach (var activity in activities)
        {
            if (!activityDict.TryGetValue(activity.Name, out var existingActivity))
            {
                activity.ScheduleId = scheduleId;
                await _activityRepository.AddAsync(activity);
                activityDict[activity.Name] = activity;
            }
        }

        // Add new time slots
        foreach (var timeSlot in timeSlots)
        {
            if (activityDict.TryGetValue(timeSlot.Activity.Name, out var activity))
            {
                timeSlot.ActivityId = activity.Id;
                timeSlot.Activity = activity;
                timeSlot.ScheduleId = existingSchedule.Id;
                timeSlot.Schedule = existingSchedule;
                await _timeSlotRepository.AddAsync(timeSlot);
            }
            else
            {
                // This shouldn't happen if the activities are properly synchronized
                throw new Exception($"Activity '{timeSlot.Activity.Name}' not found");
            }
        }

        await _scheduleRepository.UpdateAsync(existingSchedule);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ActivityEntity>> GetActivitiesByScheduleIdAsync(Guid scheduleId)
    {
        return await _activityRepository.GetByScheduleIdAsync(scheduleId);
    }
}
