using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobApplicationStatusService
{
    Task<IEnumerable<JobApplicationStatusDto>> GetAllJobApplicationStatusAsync();
    Task<JobApplicationStatusDto> GetJobApplicationStatusByIdAsync(int jobApplicationStatusId);
    Task<ResponseDto> SubmitJobApplicationStatusAsync(JobApplicationStatusDto jobApplicationStatusDto);
    Task<ResponseDto> DeleteJobApplicationStatusAsync(int jobApplicationStatusId);
}