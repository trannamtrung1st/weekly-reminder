using System;

namespace WeeklyReminder.Domain.Entities;

public class EmailTemplateEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public Guid ActivityId { get; set; }
    public ActivityEntity Activity { get; set; }
}