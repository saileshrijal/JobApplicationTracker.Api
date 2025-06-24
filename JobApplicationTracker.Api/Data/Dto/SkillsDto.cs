namespace JobApplicationTracker.Api.Data.Dto;

public class SkillsDto
{
    public int SkillId { get; set; }
    public string SkillName { get; set; } // Name of the skill (e.g., "C#", "JavaScript")
    public string Category { get; set; } // Category of the skill (e.g., "Programming Language", "Framework", "Database")
}