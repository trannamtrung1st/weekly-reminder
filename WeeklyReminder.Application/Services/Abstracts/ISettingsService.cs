using WeeklyReminder.Domain.Models;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface ISettingsService
{
    Task<SettingsModel> GetSettingsAsync(bool getSecrets);
    Task SaveSettingsAsync(SettingsModel settings);
}