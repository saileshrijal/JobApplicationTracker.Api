using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobApplicationService
{
    Task<IEnumerable<JobApplicationDto>> GetAllJobApplicationAsync();
    Task<JobApplicationDto> GetJobApplicationByIdAsync(int jobApplicationId);
    Task<ResponseDto> SubmitJobApplicationAsync(JobApplicationDto jobApplicationDto);
    Task<ResponseDto> DeleteJobApplicationAsync(int jobApplicationId);
}