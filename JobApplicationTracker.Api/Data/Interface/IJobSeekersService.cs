using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobSeekersService
{
    Task<IEnumerable<JobSeekersDto>> GetAllJobSeekersAsync();
    Task<JobSeekersDto> GetJobSeekersByIdAsync(int jobSeekerId);
    Task<ResponseDto> SubmitJobSeekersAsync(JobSeekersDto jobSeekerDto);
    Task<ResponseDto> DeleteJobSeekersAsync(int jobSeekerId);
}