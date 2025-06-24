using JobApplicationTracker.Api.Data.Dto;

namespace JobApplicationTracker.Api.Data.Interface;

public interface ISkillsService
{
    Task<IEnumerable<SkillsDto>> GetAllSkillsAsync();
    Task<SkillsDto> GetSkillsByIdAsync(int skillsId);
    Task<ResponseDto> SubmitSkillsAsync(SkillsDto skillsDto);
    Task<ResponseDto> DeleteSkillsAsync(int skillsId);
}