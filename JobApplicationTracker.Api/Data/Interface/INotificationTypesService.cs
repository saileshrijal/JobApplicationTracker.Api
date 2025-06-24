using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface INotificationsTypesService
{
    Task<IEnumerable<NotificationTypesDto>> GetAllNotificationTypesAsync();
    Task<NotificationTypesDto> GetNotificationTypesByIdAsync(int notificationTypesId);
    Task<ResponseDto> SubmitNotificationTypesAsync(NotificationTypesDto notificationTypesDto);
    Task<ResponseDto> DeleteNotificationTypesAsync(int notificationTypesId);
}