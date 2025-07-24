using SkillHubApi.Dtos;
using SkillHubApi.Models;

namespace SkillHubApi.Services
{
    public interface ILessonEnrollmentService
    {
        Task<IEnumerable<LessonEnrollmentDto>> GetAllAsync();
        Task<LessonEnrollmentDto?> GetByIdAsync(Guid id);
        Task<LessonEnrollmentDto> CreateAsync(LessonEnrollmentCreateDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}

