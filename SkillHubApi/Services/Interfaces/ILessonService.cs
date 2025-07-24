using SkillHubApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHubApi.Services
{
    public interface ILessonService
    {
        Task<LessonDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<LessonDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        Task<LessonDto> CreateAsync(LessonCreateDto lessonCreateDto);
        Task<bool> UpdateAsync(Guid id, LessonUpdateDto lessonUpdateDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
