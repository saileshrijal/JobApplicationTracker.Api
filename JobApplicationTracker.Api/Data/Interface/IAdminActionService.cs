using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IAdminActionService
{
    Task<IEnumerable<AdminActionsDto>> GetAllAdminActionAsync();
    Task<AdminActionsDto> GetAdminActionByIdAsync(int adminActionId);
    Task<ResponseDto> SubmitAdminActionAsync(AdminActionsDto adminActionDto);
    Task<ResponseDto> DeleteAdminActionAsync(int actionId);
}


