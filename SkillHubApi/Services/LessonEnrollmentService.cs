using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Interfaces;
using SkillHubApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillHubApi.Services
{
    public class LessonEnrollmentService : ILessonEnrollmentService
    {
        private readonly SkillHubDbContext _context;

        public LessonEnrollmentService(SkillHubDbContext context)
        {
            _context = context;
        }

        public async Task<LessonEnrollmentDto> GetByIdAsync(Guid id)
        {
            var enrollment = await _context.LessonEnrollments
                .Include(e => e.User)
                .Include(e => e.Lesson)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enrollment == null) return null;

            return MapToDto(enrollment);
        }

        public async Task<IEnumerable<LessonEnrollmentDto>> GetAllAsync()
        {
            var enrollments = await _context.LessonEnrollments
                .Include(e => e.User)
                .Include(e => e.Lesson)
                .ToListAsync();

            return enrollments.Select(MapToDto);
        }

        public async Task<IEnumerable<LessonEnrollmentDto>> GetByLessonIdAsync(Guid lessonId)
        {
            var enrollments = await _context.LessonEnrollments
                .Include(e => e.User)
                .Where(e => e.LessonId == lessonId)
                .ToListAsync();

            return enrollments.Select(MapToDto);
        }

        public async Task<IEnumerable<LessonEnrollmentDto>> GetByUserIdAsync(Guid userId)
        {
            var enrollments = await _context.LessonEnrollments
                .Include(e => e.Lesson)
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return enrollments.Select(MapToDto);
        }

        public async Task<LessonEnrollmentDto> CreateAsync(LessonEnrollmentCreateDto dto)
        {
            if (await IsUserEnrolledAsync(dto.UserId, dto.LessonId))
                throw new InvalidOperationException("User is already enrolled in this lesson");

            var lesson = await _context.Lessons.FindAsync(dto.LessonId);
            if (lesson == null)
                throw new KeyNotFoundException("Lesson not found");

            var currentEnrollments = await GetEnrollmentCountAsync(dto.LessonId);
            if (currentEnrollments >= lesson.Capacity)
                throw new InvalidOperationException("Lesson capacity reached");

            var enrollment = new LessonEnrollment
            {
                LessonId = dto.LessonId,
                UserId = dto.UserId,
                EnrolledAt = DateTime.UtcNow
            };

            _context.LessonEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return MapToDto(enrollment);
        }

        public async Task<bool> UpdateAsync(Guid id, LessonEnrollmentUpdateDto dto)
        {
            var enrollment = await _context.LessonEnrollments.FindAsync(id);
            if (enrollment == null) return false;

            if (dto.IsCompleted.HasValue)
                enrollment.IsCompleted = dto.IsCompleted.Value;

            if (dto.IsCancelled.HasValue)
                enrollment.IsCancelled = dto.IsCancelled.Value;

            _context.LessonEnrollments.Update(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var enrollment = await _context.LessonEnrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.LessonEnrollments.Remove(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsUserEnrolledAsync(Guid userId, Guid lessonId)
        {
            return await _context.LessonEnrollments
                .AnyAsync(e => e.UserId == userId && e.LessonId == lessonId);
        }

        public async Task<int> GetEnrollmentCountAsync(Guid lessonId)
        {
            return await _context.LessonEnrollments
                .Where(e => e.LessonId == lessonId)
                .CountAsync();
        }

        private static LessonEnrollmentDto MapToDto(LessonEnrollment enrollment)
        {
            return new LessonEnrollmentDto
            {
                Id = enrollment.Id,
                LessonId = enrollment.LessonId,
                UserId = enrollment.UserId,
                EnrolledAt = enrollment.EnrolledAt,
                IsCompleted = enrollment.IsCompleted,
            };
        }
    }
}