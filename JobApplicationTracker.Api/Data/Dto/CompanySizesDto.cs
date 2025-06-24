namespace JobApplicationTracker.Api.Data.Dto;
    public class CompanySizeDto
{
    public int CompanySizeId { get; set; }
    public string SizeName { get; set; }
    public string Description { get; set; }
    public int MinEmployees { get; set; }
    public int MaxEmployees { get; set; }
}
