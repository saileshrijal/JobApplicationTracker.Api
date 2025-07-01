namespace JobApplicationTracker.Api.Data.Interface
{
    public interface ICookieService
    {
        void AppendCookies(HttpResponse response, string authToken);
    }
}
