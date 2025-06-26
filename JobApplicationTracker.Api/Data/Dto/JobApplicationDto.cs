namespace JobApplicationTracker.Api.Data.Dto;

public class JobApplicationDto
{
    public int JobApplicationId { get; set; }
    public int JobId { get; set; }
    public string JobTittle { get; set; }
    public string JobSeekerId { get; set; }
    public string CoverLetter { get; set; }
    public DateTime ApplicationDate { get; set; }

    public int StatusId { get; set; } //  Applied, Interviewing, Offer, Rejected

    public DateTime AppliedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
