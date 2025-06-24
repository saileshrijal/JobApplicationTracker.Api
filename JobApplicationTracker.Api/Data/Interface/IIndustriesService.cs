using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IIndustriesService
{
    Task<IEnumerable<IndustriesDto>> GetAllIndustriesAsync();
    Task<IndustriesDto> GetIndustryByIdAsync(int industryId);
    Task<ResponseDto> SubmitIndustriesAsync(IndustriesDto industriesDto);
    Task<ResponseDto> DeleteIndustriesAsync(int industryId);
}