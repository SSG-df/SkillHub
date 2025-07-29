using SkillHubApi.Dtos;


namespace SkillHubApi.Services
{
    public interface IReportRequestService
    {
        Task<IEnumerable<ReportRequestDto>> GetAllAsync();
        Task<ReportRequestDto?> GetByIdAsync(Guid id);
        Task<ReportRequestDto> CreateAsync(ReportRequestCreateDto dto);
        Task<bool> UpdateAsync(Guid id, ReportRequestUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}