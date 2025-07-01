using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(UsersDto user);
    }
}
