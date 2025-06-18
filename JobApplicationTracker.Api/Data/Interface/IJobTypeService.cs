using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IJobTypeService
{
    Task<IEnumerable<JobTypeDto>> GetAllJobTypesAsync();
    Task<JobTypeDto> GetJobTypeByIdAsync(int jobTypeId);
    Task<ResponseDto> SubmitJobTypeAsync(JobTypeDto jobTypeDto);
    Task<ResponseDto> DeleteJobTypeAsync(int jobTypeId);
}