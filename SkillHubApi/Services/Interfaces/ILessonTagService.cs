using SkillHubApi.Dtos;

namespace SkillHubApi.Services
{
    public interface ILessonTagService
    {
        Task<IEnumerable<LessonTagDto>> GetAllAsync();
        Task<LessonTagDto?> GetByIdAsync(Guid lessonId, Guid tagId);
        Task<LessonTagDto> CreateAsync(LessonTagCreateDto dto);
        Task<bool> DeleteAsync(Guid lessonId, Guid tagId);
    }
}
