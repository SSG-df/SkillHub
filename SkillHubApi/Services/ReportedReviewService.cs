using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SkillHubApi.Services
{
    public class ReportedReviewService : IReportedReviewService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportedReviewService(
            SkillHubDbContext context, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ReportedReviewDto>> GetAllAsync()
        {
            var reports = await _context.ReportedReviews
                .Include(r => r.Review)
                .Include(r => r.Reporter)
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReportedReviewDto>>(reports);
        }

        public async Task<ReportedReviewDto?> GetByIdAsync(Guid id)
        {
            var report = await _context.ReportedReviews
                .Include(r => r.Review)
                .Include(r => r.Reporter)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            return report == null ? null : _mapper.Map<ReportedReviewDto>(report);
        }

        public async Task<ReportedReviewDto> CreateAsync(ReportedReviewCreateDto dto)
        {
            var reviewExists = await _context.Reviews.AnyAsync(r => r.Id == dto.ReviewId);
            if (!reviewExists)
                throw new ArgumentException("Review not found");

            var report = new ReportedReview
            {
                ReviewId = dto.ReviewId,
                Reason = dto.Reason,
                ReporterId = GetCurrentUserId(),
                ReportedAt = DateTime.UtcNow
            };

            _context.ReportedReviews.Add(report);
            await _context.SaveChangesAsync();
            
            return await GetByIdAsync(report.Id);
        }

        public async Task<bool> UpdateAsync(Guid id, ReportedReviewUpdateDto dto)
        {
            var report = await _context.ReportedReviews.FindAsync(id);
            if (report == null) return false;

            if (report.ReporterId != GetCurrentUserId())
                return false;

            report.Reason = dto.Reason;
            report.ReportedAt = DateTime.UtcNow; 
            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var report = await _context.ReportedReviews.FindAsync(id);
            if (report == null) return false;

            var currentUserId = GetCurrentUserId();
            if (report.ReporterId != currentUserId && !IsAdmin())
                return false;

            _context.ReportedReviews.Remove(report);
            await _context.SaveChangesAsync();
            return true;
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
    }
}