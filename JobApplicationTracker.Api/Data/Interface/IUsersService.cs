using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IUsersService
{
    Task<IEnumerable<UsersDto>> GetAllUsersAsync();
    Task<UsersDto> GetUsersByIdAsync(int userId);
    Task<ResponseDto> SubmitUsersAsync(UsersDto userDto);
    Task<ResponseDto> DeleteUsersAsync(int userId);
}