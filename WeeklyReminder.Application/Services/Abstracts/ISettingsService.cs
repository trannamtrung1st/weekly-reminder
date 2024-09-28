using WeeklyReminder.Application.Models;
using WeeklyReminder.Domain.Models;

namespace WeeklyReminder.Application.Services.Abstracts;

public interface ISettingsService
{
    Task<SystemCredentials> GetSystemCredentialsAsync();
    Task SaveSystemCredentialsAsync(SystemCredentials credentials);
    AppSettings GetAppSettings();
}
