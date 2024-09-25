using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;
using WeeklyReminder.Domain.Services;

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
        var (schedule, timeSlots, activityNames) = await _timetableParser.ParseTimetable(timetableStream);
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

        foreach (var timeSlot in timeSlots)
        {
            var activity = await _activityRepository.GetOrCreateByNameAsync(timeSlot.Activity.Name);
            timeSlot.ActivityId = activity.Id;
            timeSlot.Activity = activity;
            timeSlot.ScheduleId = schedule.Id;
            timeSlot.Schedule = schedule;
            await _timeSlotRepository.AddAsync(timeSlot);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
