using System.Security.Cryptography;
using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Models;

namespace WeeklyReminder.Infrastructure.Services;

public class SettingsService : ISettingsService
{
    private const string SettingsFile = "settings.txt";

    public async Task<SettingsModel> GetSettingsAsync()
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, SettingsFile);
        var lines = await File.ReadAllLinesAsync(settingsPath);
        var settings = new SettingsModel();

        foreach (var line in lines)
        {
            var parts = line.Split('=');
            if (parts.Length == 2)
            {
                var key = parts[0].Trim();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "username":
                        settings.Username = value;
                        break;
                    case "password":
                        settings.Password = value;
                        break;
                    case "email":
                        settings.Email = value;
                        break;
                    case "apppassword":
                        settings.AppPassword = value;
                        break;
                }
            }
        }

        return settings;
    }

    public async Task SaveSettingsAsync(SettingsModel settings)
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, SettingsFile);
        var lines = new[]
        {
            $"username={settings.Username}",
            $"password={settings.Password}",
            $"email={settings.Email}",
            $"apppassword={settings.AppPassword}"
        };

        await File.WriteAllLinesAsync(settingsPath, lines);
    }
}