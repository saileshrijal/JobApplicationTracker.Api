namespace JobApplicationTracker.Api.Data.Dto;

public class UserTypesDto
{
    public int UserTypeId { get; set; }
    public string TypeName { get; set; } // Name of the skill (e.g., "C#", "JavaScript")
    public string Description { get; set; } // Category of the skill (e.g., "Programming Language", "Framework", "Database")
}