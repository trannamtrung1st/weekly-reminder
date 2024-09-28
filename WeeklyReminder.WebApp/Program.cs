using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Application.Services;
using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Repositories;
using WeeklyReminder.Infrastructure.Services;
using WeeklyReminder.Persistence;
using WeeklyReminder.Persistence.Repositories;
using WeeklyReminder.Persistence.Services;
using WeeklyReminder.WebApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using WeeklyReminder.WebApp.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using WeeklyReminder.Domain.Services.Abstracts;
using WeeklyReminder.Application.Repositories;
using Hangfire;
using Hangfire.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthenticationCore()
    .AddHttpContextAccessor();

builder.Services.AddDbContext<WeeklyReminderDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddScoped<IUnitOfWork, UnitOfWork>()
    .AddScoped<IScheduleRepository, ScheduleRepository>()
    .AddScoped<ITimeSlotRepository, TimeSlotRepository>()
    .AddScoped<IActivityRepository, ActivityRepository>()
    .AddScoped<IUserRepository, UserRepository>()
    .AddScoped<IEmailTemplateRepository, EmailTemplateRepository>()
    .AddScoped<IReminderRepository, ReminderRepository>();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/access-denied";
    });

builder.Services.AddAuthorization();

builder.Services
    .AddScoped<IScheduleService, ScheduleService>()
    .AddScoped<IWeeklyTimetableParser, WeeklyTimetableParser>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<ISettingsService, SettingsService>()
    .AddScoped<IEmailService, EmailService>()
    .AddScoped<IEmailTemplateService, EmailTemplateService>()
    .AddScoped<IActivityService, ActivityService>()
    .AddScoped<IReminderService, ReminderService>();

builder.Services
    .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseInMemoryStorage());

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseAntiforgery();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.UseHangfireDashboard();

await ApplyDatabaseMigration(app);
await ScheduleHangfireJobs(app);

app.Run();

static async Task ApplyDatabaseMigration(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<WeeklyReminderDbContext>();
        var retryCount = 0;
        const int maxRetries = 5;
        while (retryCount < maxRetries)
        {
            try
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migration completed successfully.");
                break;
            }
            catch (Exception)
            {
                retryCount++;
                if (retryCount >= maxRetries)
                {
                    throw;
                }
                logger.LogWarning($"Database migration failed. Retrying in 5 seconds. Attempt {retryCount} of {maxRetries}");
                await Task.Delay(5000);
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

static async Task ScheduleHangfireJobs(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var settingsService = scope.ServiceProvider.GetRequiredService<ISettingsService>();
    var settings = await settingsService.GetSettingsAsync(getSecrets: false);
    var serverTimeZone = TimeZoneInfo.FindSystemTimeZoneById(settings.ServerTimeZone);

    RecurringJob.AddOrUpdate<IReminderService>(
        "check-upcoming-activities",
        service => service.CheckUpcomingActivitiesAndSendReminders(),
        "*/1 * * * *",
        options: new RecurringJobOptions
        {
            TimeZone = serverTimeZone
        });
}
