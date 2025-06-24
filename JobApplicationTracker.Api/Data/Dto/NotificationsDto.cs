namespace JobApplicationTracker.Api.Data.Dto;

public class NotificationsDto
{
    public int NotificationId { get; set; }

 
    public string UserId { get; set; } // Foreign key to UsersDto
    public string Title { get; set; }
    public string Message { get; set; }
    public int NotificationTypeId { get; set; } // Foreign key to NotificationTypesDto
    public bool IsRead { get; set; } // Indicates if the notification has been read
    public DateTime CreatedAt { get; set; } // Timestamp of when the notification was created
}