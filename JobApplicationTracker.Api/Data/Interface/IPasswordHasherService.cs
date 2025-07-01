namespace JobApplicationTracker.Api.Data.Interface
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
    }
}
