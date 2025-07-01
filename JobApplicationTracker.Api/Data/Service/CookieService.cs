using JobApplicationTracker.Api.Configuration;
using JobApplicationTracker.Api.Data.Interface;
using Microsoft.Extensions.Options;

namespace JobApplicationTracker.Api.Data.Service
{
    public class CookieService : ICookieService
    {
        private readonly JwtSettings _jwtSettings;

        public CookieService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        public void AppendCookies(HttpResponse response, string authToken)
        {
            CookieOptions options = BuildCookies();
            response.Cookies.Append("authToken",authToken);
        }


        private CookieOptions BuildCookies()
        {
            return new CookieOptions()
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwtSettings.ExpireMinutes))
            };
        }
    }
}
