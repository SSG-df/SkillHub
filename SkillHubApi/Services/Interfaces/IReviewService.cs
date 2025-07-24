using SkillHubApi.Dtos;

namespace SkillHubApi.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<ReviewDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ReviewDto>> GetByLessonIdAsync(Guid lessonId);
        Task<ReviewDto> CreateAsync(ReviewCreateDto reviewDto);
        Task<bool> UpdateAsync(Guid id, ReviewUpdateDto reviewDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
