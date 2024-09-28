using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;
using WeeklyReminder.Domain.Repositories;
using WeeklyReminder.Domain.Services.Abstracts;

namespace WeeklyReminder.Application.Services;

public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivityService(IActivityRepository activityRepository, IUnitOfWork unitOfWork)
    {
        _activityRepository = activityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ActivityEntity> GetActivityByIdAsync(Guid id)
    {
        return await _activityRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<ActivityEntity>> GetAllActivitiesAsync()
    {
        return await _activityRepository.GetAllAsync();
    }

    public async Task<ActivityEntity> CreateActivityAsync(ActivityEntity activity)
    {
        await _activityRepository.AddAsync(activity);
        await _unitOfWork.SaveChangesAsync();
        return activity;
    }

    public async Task UpdateActivityAsync(ActivityEntity activity)
    {
        await _activityRepository.UpdateAsync(activity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteActivityAsync(Guid id)
    {
        await _activityRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}