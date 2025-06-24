using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobsService
{
    Task<IEnumerable<JobsDto>> GetAllJobsAsync();
    Task<JobsDto> GetJobsByIdAsync(int jobId);
    Task<ResponseDto> SubmitJobAsync(JobsDto jobsDto);
    Task<ResponseDto> DeleteJobAsync(int jobsId);
}