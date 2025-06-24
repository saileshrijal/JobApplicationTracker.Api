namespace JobApplicationTracker.Api.Data.Dto;

public class CompaniesDto
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string CompanyLogo { get; set; }
    public int? IndustryId { get; set; } // Nullable to allow for optional industry association
    public int? CompanySizeId { get; set; } // Nullable to allow for optional company size
    public string Website { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}