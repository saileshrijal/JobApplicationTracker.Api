namespace JobApplicationTracker.Api.Data.Dto;

public class JobSeekerSkillsDto
{
    public int JobSeekerSkillsId { get; set; }
    public int JobSeekerId{ get; set; }
    public int SkillId { get; set; }
    public int ProficiencyLevel { get; set; } // 1 to 5 scale
}
