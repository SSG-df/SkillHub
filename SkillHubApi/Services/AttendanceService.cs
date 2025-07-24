using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SkillHubApi.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly SkillHubDbContext _context;

        public AttendanceService(SkillHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
        {
            return await _context.Attendances
                .Select(a => new AttendanceDto
                {
                    Id = a.Id,
                    LessonEnrollmentId = a.EnrollmentId,
                    Date = a.AttendedAt,
                    IsPresent = a.IsPresent
                }).ToListAsync();
        }

        public async Task<AttendanceDto?> GetByIdAsync(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return null;

            return new AttendanceDto
            {
                Id = attendance.Id,
                LessonEnrollmentId = attendance.EnrollmentId,
                Date = attendance.AttendedAt,
                IsPresent = attendance.IsPresent
            };
        }

        public async Task<AttendanceDto> AddAsync(AttendanceCreateDto dto)
        {
            var attendance = new Attendance
            {
                Id = Guid.NewGuid(),
                EnrollmentId = dto.LessonEnrollmentId,
                AttendedAt = dto.Date,
                IsPresent = dto.IsPresent
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return new AttendanceDto
            {
                Id = attendance.Id,
                LessonEnrollmentId = attendance.EnrollmentId,
                Date = attendance.AttendedAt,
                IsPresent = attendance.IsPresent
            };
        }

        public async Task<bool> UpdateAsync(Guid id, AttendanceUpdateDto dto)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return false;

            attendance.AttendedAt = dto.Date;
            attendance.IsPresent = dto.IsPresent;

            _context.Attendances.Update(attendance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null) return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
