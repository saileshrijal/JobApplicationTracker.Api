using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface IAdminLogService
{
    Task<IEnumerable<AdminLogsDto>> GetAllAdminLogsAsync();
    Task<AdminLogsDto> GetAdminLogsByIdAsync(int adminLogId);
    Task<ResponseDto> SubmitAdminLogsAsync(AdminLogsDto adminLogsDto);

    Task<ResponseDto> DeleteAdminLogsAsync(int logId);
}