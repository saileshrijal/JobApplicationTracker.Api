namespace JobApplicationTracker.Api.Configuration
{
    public class JwtSettings
    {
        public string Audience { get; set; } = null!;
        public string Issuer{ get; set; } = null!;
        public string Key { get; set; } = null!;
        public int ExpireMinutes { get; set; } 

    }
}
