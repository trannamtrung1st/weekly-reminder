using Microsoft.EntityFrameworkCore;
using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.Persistence;

public class WeeklyReminderDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ScheduleEntity> Schedules { get; set; }
    public DbSet<ActivityEntity> Activities { get; set; }
    public DbSet<TimeSlotEntity> TimeSlots { get; set; }
    public DbSet<EmailTemplateEntity> EmailTemplates { get; set; }
    public DbSet<ReminderEntity> Reminders { get; set; }

    public WeeklyReminderDbContext(DbContextOptions<WeeklyReminderDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ActivityEntity>()
            .HasOne(a => a.Schedule)
            .WithMany(s => s.Activities)
            .HasForeignKey(a => a.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TimeSlotEntity>()
            .HasOne(ts => ts.Schedule)
            .WithMany(s => s.TimeSlots)
            .HasForeignKey(ts => ts.ScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TimeSlotEntity>()
            .HasOne(ts => ts.Activity)
            .WithMany(a => a.TimeSlots)
            .HasForeignKey(ts => ts.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmailTemplateEntity>()
            .HasOne(et => et.Activity)
            .WithMany(a => a.EmailTemplates)
            .HasForeignKey(et => et.ActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ReminderEntity>()
            .HasOne(r => r.TimeSlot)
            .WithMany(ts => ts.Reminders)
            .HasForeignKey(r => r.TimeSlotId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}