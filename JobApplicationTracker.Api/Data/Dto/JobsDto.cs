namespace JobApplicationTracker.Api.Data.Dto;

public class JobsDto
{
    public int JobId { get; set; }
    public int CompanyUserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }
    public string Location { get; set; }
    public bool IsActive { get; set; }
    public int JobTypeId { get; set; }
    public int SalaryMin { get; set; }
    public int SalaryMax { get; set; }
    public int ExperienceLevelId { get; set; } // Nullable to allow for optional experience level
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime Deadline { get; set; } // Deadline for job application
}

