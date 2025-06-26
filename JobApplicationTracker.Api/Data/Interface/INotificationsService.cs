using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface INotificationsService
{
    Task<IEnumerable<NotificationsDto>> GetAllNotificationsAsync();
    Task<NotificationsDto> GetNotificationsByIdAsync(int notificationsId);
    Task<ResponseDto> SubmitNotificationsAsync(NotificationsDto notificationsDto);
    Task<ResponseDto> DeleteNotificationsAsync(int notificationsId);
}