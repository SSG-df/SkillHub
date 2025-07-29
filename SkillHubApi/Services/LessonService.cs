using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHubApi.Services
{
    public class LessonService : ILessonService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;

        public LessonService(SkillHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LessonDto?> GetByIdAsync(Guid id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Mentor)
                .Include(l => l.LessonTags)
                .FirstOrDefaultAsync(l => l.Id == id);

            return lesson == null ? null : _mapper.Map<LessonDto>(lesson);
        }

        public async Task<IEnumerable<LessonDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            var lessons = await _context.Lessons
                .Include(l => l.Mentor)
                .Include(l => l.LessonTags)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }

        public async Task<LessonDto> CreateAsync(LessonCreateDto dto)
        {
            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime должен быть раньше EndTime");

            if (dto.StartTime < DateTime.UtcNow)
                throw new ArgumentException("StartTime не может быть в прошлом");

            var lesson = _mapper.Map<Lesson>(dto);
            lesson.StartTime = dto.StartTime.ToUniversalTime();
            lesson.EndTime = dto.EndTime.ToUniversalTime();

            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task<bool> UpdateAsync(Guid id, LessonUpdateDto dto)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return false;

            if (dto.StartTime >= dto.EndTime)
                throw new ArgumentException("StartTime должен быть раньше EndTime");

            if (dto.StartTime < DateTime.UtcNow)
                throw new ArgumentException("StartTime не может быть в прошлом");

            _mapper.Map(dto, lesson);
            lesson.StartTime = dto.StartTime.ToUniversalTime();
            lesson.EndTime = dto.EndTime.ToUniversalTime();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return false;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }
         public async Task<IEnumerable<LessonDto>> SearchAsync(
            string? searchTerm,
            Guid? mentorId,
            DateTime? startDate,
            DateTime? endDate,
            IEnumerable<Guid>? tagIds,
            DifficultyLevel? difficulty,
            int? minCapacity,
            int pageNumber = 1,
            int pageSize = 20)
        {
            var query = _context.Lessons
                .Include(l => l.Mentor)
                .Include(l => l.LessonTags)
                    .ThenInclude(lt => lt.Tag)
                .Include(l => l.Enrollments)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(l => 
                    l.Title.Contains(searchTerm) || 
                    l.Description.Contains(searchTerm));
            }

            if (mentorId.HasValue)
            {
                query = query.Where(l => l.MentorId == mentorId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(l => l.StartTime >= startDate.Value.ToUniversalTime());
            }

            if (endDate.HasValue)
            {
                query = query.Where(l => l.EndTime <= endDate.Value.ToUniversalTime());
            }

            if (tagIds != null && tagIds.Any())
            {
                query = query.Where(l => l.LessonTags.Any(lt => tagIds.Contains(lt.TagId)));
            }

            if (difficulty.HasValue)
            {
                query = query.Where(l => l.Difficulty == difficulty.Value);
            }

            if (minCapacity.HasValue)
            {
                query = query.Where(l => l.Capacity >= minCapacity.Value);
            }

            var lessons = await query
                .OrderBy(l => l.StartTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LessonDto>>(lessons);
        }
    }
}
