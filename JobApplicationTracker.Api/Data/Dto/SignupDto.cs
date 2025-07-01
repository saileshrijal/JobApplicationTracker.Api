namespace JobApplicationTracker.Api.Data.Dto
{
    public class SignupDto
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
