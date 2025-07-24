using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;

namespace SkillHubApi.Services
{
    public class LessonEnrollmentService : ILessonEnrollmentService
    {
        private readonly SkillHubDbContext _context;

        public LessonEnrollmentService(SkillHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LessonEnrollmentDto>> GetAllAsync()
        {
            return await _context.LessonEnrollments
                .Select(e => new LessonEnrollmentDto
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    LessonId = e.LessonId,
                    EnrolledAt = e.EnrolledAt
                })
                .ToListAsync();
        }

        public async Task<LessonEnrollmentDto?> GetByIdAsync(Guid id)
        {
            var enrollment = await _context.LessonEnrollments.FindAsync(id);
            if (enrollment == null) return null;

            return new LessonEnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                LessonId = enrollment.LessonId,
                EnrolledAt = enrollment.EnrolledAt
            };
        }

        public async Task<LessonEnrollmentDto> CreateAsync(LessonEnrollmentCreateDto dto)
        {
            var enrollment = new LessonEnrollment
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                LessonId = dto.LessonId,
                EnrolledAt = DateTime.UtcNow
            };

            _context.LessonEnrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return new LessonEnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                LessonId = enrollment.LessonId,
                EnrolledAt = enrollment.EnrolledAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var enrollment = await _context.LessonEnrollments.FindAsync(id);
            if (enrollment == null) return false;

            _context.LessonEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
