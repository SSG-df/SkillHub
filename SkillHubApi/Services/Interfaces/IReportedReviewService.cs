using SkillHubApi.Dtos;

namespace SkillHubApi.Services
{
    public interface IReportedReviewService
    {
        Task<IEnumerable<ReportedReviewDto>> GetAllAsync();
        Task<ReportedReviewDto?> GetByIdAsync(Guid id);
        Task<ReportedReviewDto> CreateAsync(ReportedReviewCreateDto dto);
        Task<bool> UpdateAsync(Guid id, ReportedReviewUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        
    }
}
