using SkillHubApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHubApi.Interfaces
{
    public interface ILessonEnrollmentService
    {
        Task<LessonEnrollmentDto> GetByIdAsync(Guid id);
        Task<IEnumerable<LessonEnrollmentDto>> GetAllAsync();
        Task<IEnumerable<LessonEnrollmentDto>> GetByLessonIdAsync(Guid lessonId);
        Task<IEnumerable<LessonEnrollmentDto>> GetByUserIdAsync(Guid userId);
        Task<LessonEnrollmentDto> CreateAsync(LessonEnrollmentCreateDto dto);
        Task<bool> UpdateAsync(Guid id, LessonEnrollmentUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> IsUserEnrolledAsync(Guid userId, Guid lessonId);
        Task<int> GetEnrollmentCountAsync(Guid lessonId);
    }
}