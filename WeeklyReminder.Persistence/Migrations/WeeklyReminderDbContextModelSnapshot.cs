﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeeklyReminder.Persistence;

#nullable disable

namespace WeeklyReminder.Persistence.Migrations
{
    [DbContext(typeof(WeeklyReminderDbContext))]
    partial class WeeklyReminderDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ActivityEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.EmailTemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ReminderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConfirmationToken")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsResolved")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TimeSlotId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TimeSlotId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ScheduleEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsReminderEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeSpan>("TimeInterval")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.TimeSlotEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActivityId")
                        .HasColumnType("TEXT");

                    b.Property<int>("DoW")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("TEXT");

                    b.Property<double>("StartTime")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("TimeSlots");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ActivityEntity", b =>
                {
                    b.HasOne("WeeklyReminder.Domain.Entities.ScheduleEntity", "Schedule")
                        .WithMany("Activities")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.EmailTemplateEntity", b =>
                {
                    b.HasOne("WeeklyReminder.Domain.Entities.ActivityEntity", "Activity")
                        .WithMany("EmailTemplates")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ReminderEntity", b =>
                {
                    b.HasOne("WeeklyReminder.Domain.Entities.TimeSlotEntity", "TimeSlot")
                        .WithMany("Reminders")
                        .HasForeignKey("TimeSlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TimeSlot");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ScheduleEntity", b =>
                {
                    b.HasOne("WeeklyReminder.Domain.Entities.UserEntity", "User")
                        .WithMany("Schedules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.TimeSlotEntity", b =>
                {
                    b.HasOne("WeeklyReminder.Domain.Entities.ActivityEntity", "Activity")
                        .WithMany("TimeSlots")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WeeklyReminder.Domain.Entities.ScheduleEntity", "Schedule")
                        .WithMany("TimeSlots")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ActivityEntity", b =>
                {
                    b.Navigation("EmailTemplates");

                    b.Navigation("TimeSlots");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.ScheduleEntity", b =>
                {
                    b.Navigation("Activities");

                    b.Navigation("TimeSlots");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.TimeSlotEntity", b =>
                {
                    b.Navigation("Reminders");
                });

            modelBuilder.Entity("WeeklyReminder.Domain.Entities.UserEntity", b =>
                {
                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
