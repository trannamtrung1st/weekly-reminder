using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Models;
using Microsoft.Extensions.Configuration;
using WeeklyReminder.Application.Models;

namespace WeeklyReminder.Infrastructure.Services;

public class SettingsService : ISettingsService
{
    private const string CredentialsFile = "credentials.txt";
    private readonly IConfiguration _configuration;

    public SettingsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<SystemCredentials> GetSystemCredentialsAsync()
    {
        var credentialsPath = Path.Combine(AppContext.BaseDirectory, CredentialsFile);
        if (!File.Exists(credentialsPath))
        {
            var defaultCredentials = new SystemCredentials()
            {
                Email = string.Empty,
                Username = "admin",
                Password = null
            };
            return defaultCredentials;
        }

        var lines = await File.ReadAllLinesAsync(credentialsPath);
        var credentials = new SystemCredentials();

        foreach (var line in lines)
        {
            var parts = line.Split('=');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key.ToLower())
                {
                    case "username":
                        credentials.Username = value;
                        break;
                    case "password":
                        credentials.Password = value;
                        break;
                    case "email":
                        credentials.Email = value;
                        break;
                    case "apppassword":
                        credentials.AppPassword = value;
                        break;
                }
            }
        }

        return credentials;
    }

    public async Task SaveSystemCredentialsAsync(SystemCredentials newCredentials)
    {
        var currentCredentials = await GetSystemCredentialsAsync();

        var updatedCredentials = new SystemCredentials
        {
            Username = newCredentials.Username,
            Email = newCredentials.Email,
            Password = string.IsNullOrEmpty(newCredentials.Password) ? currentCredentials.Password : newCredentials.Password,
            AppPassword = string.IsNullOrEmpty(newCredentials.AppPassword) ? currentCredentials.AppPassword : newCredentials.AppPassword
        };

        var credentialsPath = Path.Combine(AppContext.BaseDirectory, CredentialsFile);
        var lines = new[]
        {
            $"username={updatedCredentials.Username}",
            $"password={updatedCredentials.Password}",
            $"email={updatedCredentials.Email}",
            $"apppassword={updatedCredentials.AppPassword}"
        };

        await File.WriteAllLinesAsync(credentialsPath, lines);
    }

    public AppSettings GetAppSettings()
    {
        return _configuration.GetSection("AppSettings").Get<AppSettings>();
    }
}