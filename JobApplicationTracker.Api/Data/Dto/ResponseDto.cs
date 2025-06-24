namespace JobApplicationTracker.Api.Data.Dto;

public class ResponseDto
{
    

    public int id { get; set; }
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}