using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobSeekerExperienceService
{
    Task<IEnumerable<JobSeekerExperienceDto>> GetAllJobSeekerExperienceAsync();
    Task<JobSeekerExperienceDto> GetJobSeekerExperienceByIdAsync(int jobSeekerExperienceId);
    Task<ResponseDto> SubmitJobSeekerExperienceAsync(JobSeekerExperienceDto jobSeekerExperienceDto);
    Task<ResponseDto> DeleteJobSeekerExperienceAsync(int jobSeekerExperienceId);
}