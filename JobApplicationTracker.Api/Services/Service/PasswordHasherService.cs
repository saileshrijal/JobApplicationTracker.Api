using JobApplicationTracker.Api.Services.Interfaces;

namespace JobApplicationTracker.Api.Services.Service
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
