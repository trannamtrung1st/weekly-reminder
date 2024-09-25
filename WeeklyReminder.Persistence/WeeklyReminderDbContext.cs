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
    public DbSet<TimeSlotEntity> TimeSlots { get; set; }
    public DbSet<ActivityEntity> Activities { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ScheduleEntity>()
            .HasMany(s => s.TimeSlots)
            .WithOne(s => s.Schedule)
            .HasForeignKey(s => s.ScheduleId);

        modelBuilder.Entity<UserEntity>()
            .HasMany(u => u.Schedules)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId);

        modelBuilder.Entity<TimeSlotEntity>()
            .HasOne(ts => ts.Activity)
            .WithMany()
            .HasForeignKey(ts => ts.ActivityId);
    }
}