using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobSeekersEducationService
{
    Task<IEnumerable<JobSeekerEducationDto>> GetAllJobSeekerEducationAsync();
    Task<JobSeekerEducationDto> GetJobSeekerEducationByIdAsync(int jobSeekerEducationId);
    Task<ResponseDto> SubmitJobSeekerEducationAsync(JobSeekerEducationDto jobSeekerEducationDto);
    Task<ResponseDto> DeleteJobSeekerEducationAsync(int jobSeekerEducationId);
}