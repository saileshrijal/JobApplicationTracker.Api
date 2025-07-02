namespace JobApplicationTracker.Api.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
    }
}
