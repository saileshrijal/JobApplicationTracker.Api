using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IUsersTypeService
{
    Task<IEnumerable<UserTypesDto>> GetAllUserTypesAsync();
    Task<UserTypesDto> GetUserTypesByIdAsync(int userTypesId);
    Task<ResponseDto> SubmitUserTypesAsync(UserTypesDto userTypesDto);
    Task<ResponseDto> DeleteUserTypesAsync(int userTypesId);
}