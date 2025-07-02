namespace JobApplicationTracker.Api.Services.Interfaces
{
    public interface ICookieService
    {
        void AppendCookies(HttpResponse response, string authToken);
    }
}
