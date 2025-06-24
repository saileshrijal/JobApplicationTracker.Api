namespace JobApplicationTracker.Api.Data.Dto;

public class JobSeekerEducationDto
{
    public int EducationId { get; set; }
    public string JobSeekerId { get; set; } // Foreign key to JobSeeker

    public string Institution { get; set; }

    public string Degree { get; set; }
    public string FieldOfStudy { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } // Nullable to allow for ongoing education
    public double? GPA { get; set; } // Nullable to allow for optional GPA

}
    
    