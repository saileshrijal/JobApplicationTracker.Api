using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Services.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(UsersDto user);
    }
}
