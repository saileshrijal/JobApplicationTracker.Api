using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface ICompaniesService
{
    Task<IEnumerable<CompaniesDto>> GetAllCompaniesAsync();
    Task<CompaniesDto> GetCompaniesByIdAsync(int companiesId);
    Task<ResponseDto> SubmitCompaniesAsync(CompaniesDto companiesDto);

    Task<ResponseDto> DeleteCompanyAsync(int companiesId);
    Task<object?> DeleteCompaniesAsync(int id);
}