namespace JobApplicationTracker.Api.Data.Dto;

public class NotificationTypesDto
{
    public int NotificationTypeId { get; set; }
    public string TypeName { get; set; } // Name of the notification type (e.g., "Job Application", "Interview Reminder")
    public string Description { get; set; } // Description of the notification type
    public int Priority { get; set; } // Priority level of the notification type (e.g., 1 for high, 2 for medium, 3 for low)


 }
