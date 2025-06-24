namespace JobApplicationTracker.Api.Data.Dto;

public class JobSeekerExperienceDto
{
    public int ExperienceId { get; set; }
 
    public int JobSeekerId { get; set; } // Foreign key 
    public string CompanyName { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // Nullable to allow for ongoing experience
    public string Description { get; set; } // Optional description of the experience
}