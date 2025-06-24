using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface ICompanySizesService
{
    Task<IEnumerable<CompanySizeDto>> GetAllCompanySizeAsync();
    Task<CompanySizeDto> GetCompanySizeByIdAsync(int companySizeId);
    Task<ResponseDto> SubmitCompanySizeAsync(CompanySizeDto companySizeDto);

    Task<ResponseDto> DeleteCompanySizeAsync(int companySizeId);
  
}