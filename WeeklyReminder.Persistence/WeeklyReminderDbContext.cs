using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Persistence;

public class WeeklyReminderDbContext : DbContext
{
    public WeeklyReminderDbContext(DbContextOptions<WeeklyReminderDbContext> options)
        : base(options)
    {
    }

    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<TimeSlot> TimeSlots { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Schedule>()
            .HasMany(s => s.Days)
            .WithOne()
            .HasForeignKey("ScheduleId");

        modelBuilder.Entity<Day>()
            .HasMany(d => d.TimeSlots)
            .WithOne()
            .HasForeignKey("DayId");

        modelBuilder.Entity<TimeSlot>()
            .HasOne(ts => ts.Activity)
            .WithMany()
            .HasForeignKey("ActivityId");
    }
}