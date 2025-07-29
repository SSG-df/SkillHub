using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SkillHubApi.Services
{
    public class ReportRequestService : IReportRequestService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportRequestService(
            SkillHubDbContext context, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ReportRequestDto>> GetAllAsync()
        {
            var reports = await _context.ReportRequests
                .Include(r => r.RequestedBy)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReportRequestDto>>(reports);
        }

        public async Task<ReportRequestDto?> GetByIdAsync(Guid id)
        {
            var report = await _context.ReportRequests
                .Include(r => r.RequestedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            return report == null ? null : _mapper.Map<ReportRequestDto>(report);
        }

        public async Task<ReportRequestDto> CreateAsync(ReportRequestCreateDto dto)
        {
            Guid? lessonId = null;
            Guid? userId = null;

            if (!string.IsNullOrEmpty(dto.LessonId))
            {
                if (!Guid.TryParse(dto.LessonId, out var lessonGuid))
                throw new ArgumentException("Invalid LessonId format");
                lessonId = lessonGuid;
            }

            if (!string.IsNullOrEmpty(dto.UserId))
            {
                if (!Guid.TryParse(dto.UserId, out var userGuid))
                throw new ArgumentException("Invalid UserId format");
                userId = userGuid;
            }
            if (lessonId.HasValue && userId.HasValue)
                throw new ArgumentException("Specify only LessonId OR UserId");

            if (!lessonId.HasValue && !userId.HasValue)
                throw new ArgumentException("Either LessonId or UserId must be provided");

            var report = new ReportRequest
        {
            Reason = dto.Reason,
            LessonId = lessonId,
            UserId = userId,
            RequestedById = GetCurrentUserId(),
            RequestedAt = DateTime.UtcNow
        };

            _context.ReportRequests.Add(report);
            await _context.SaveChangesAsync();
    
            return _mapper.Map<ReportRequestDto>(report);
        }

        public async Task<bool> UpdateAsync(Guid id, ReportRequestUpdateDto dto)
        {
            var report = await _context.ReportRequests.FindAsync(id);
            if (report == null) return false;

            if (!HasPermissionToModify(report))
                return false;

            _mapper.Map(dto, report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var report = await _context.ReportRequests.FindAsync(id);
            if (report == null) return false;

            if (!HasPermissionToModify(report))
                return false;

            _context.ReportRequests.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        private void ValidateReportRequest(ReportRequestCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new ArgumentException("Reason is required");

                bool hasLessonId = !string.IsNullOrEmpty(dto.LessonId) && Guid.TryParse(dto.LessonId, out _);
                bool hasUserId = !string.IsNullOrEmpty(dto.UserId) && Guid.TryParse(dto.UserId, out _);

            if (hasLessonId && hasUserId)
                throw new ArgumentException("Specify either LessonId OR UserId, not both");

            if (!hasLessonId && !hasUserId)
                throw new ArgumentException("Either LessonId or UserId must be specified");
        }

        private Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var result) ? result : Guid.Empty;
        }

        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
        }

        private bool HasPermissionToModify(ReportRequest report)
        {
            var currentUserId = GetCurrentUserId();
            return report.RequestedById == currentUserId || IsAdmin();
        }
    }
}