using System;

namespace WeeklyReminder.Domain.Entities;

public class ReminderEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TimeSlotId { get; set; }
    public TimeSlotEntity TimeSlot { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsResolved { get; set; }
    public string ConfirmationToken { get; set; }
}