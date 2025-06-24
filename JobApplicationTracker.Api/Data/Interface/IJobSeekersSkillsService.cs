using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobSeekersSkillsService
{
    Task<IEnumerable<JobSeekerSkillsDto>> GetAllJobSeekerSkillsAsync();
    Task<JobSeekerSkillsDto> GetJobSeekerSkillsByIdAsync(int jobSeekerSkillsId);
    Task<ResponseDto> SubmitJobSeekerSkillsAsync(JobSeekerSkillsDto jobSeekerSkillsDto);
    Task<ResponseDto> DeleteJobSeekerSkillsAsync(int jobSeekerSkillsId);
}