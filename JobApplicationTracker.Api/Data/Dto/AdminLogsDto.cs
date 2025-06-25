namespace JobApplicationTracker.Api.Data.Dto;

public class AdminLogsDto
{
    public int LogId { get; set; }
    public int AdminId { get; set; }
    public string ActionId { get; set; }
    public string Description { get; set; }
    public DateTime ActionDate { get; set; }
}
