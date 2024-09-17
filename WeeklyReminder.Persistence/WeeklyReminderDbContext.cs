using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Persistence;

public class WeeklyReminderDbContext : DbContext
{
    public WeeklyReminderDbContext(DbContextOptions<WeeklyReminderDbContext> options)
        : base(options)
    {
    }

    public DbSet<ScheduleEntity> Schedules { get; set; }
    public DbSet<DayEntity> Days { get; set; }
    public DbSet<TimeSlotEntity> TimeSlots { get; set; }
    public DbSet<ActivityEntity> Activities { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScheduleEntity>()
            .HasMany(s => s.Days)
            .WithOne(s => s.Schedule)
            .HasForeignKey(s => s.ScheduleId);

        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.Schedules)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<DayEntity>()
            .HasMany(d => d.TimeSlots)
            .WithOne(d => d.Day)
            .HasForeignKey(d => d.DayId);

        modelBuilder.Entity<TimeSlotEntity>()
            .HasOne(ts => ts.Activity)
            .WithMany()
            .HasForeignKey(ts => ts.ActivityId);
    }
}