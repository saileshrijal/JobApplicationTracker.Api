using JobApplicationTracker.Api.Data.Interface;

namespace JobApplicationTracker.Api.Data.Service
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
