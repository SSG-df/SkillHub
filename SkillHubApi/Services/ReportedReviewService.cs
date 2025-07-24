using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;

namespace SkillHubApi.Services
{
    public class ReportedReviewService : IReportedReviewService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;

        public ReportedReviewService(SkillHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReportedReviewDto>> GetAllAsync()
        {
            var reports = await _context.ReportedReviews.ToListAsync();
            return _mapper.Map<IEnumerable<ReportedReviewDto>>(reports);
        }

        public async Task<ReportedReviewDto?> GetByIdAsync(Guid id)
        {
            var report = await _context.ReportedReviews.FindAsync(id);
            return report == null ? null : _mapper.Map<ReportedReviewDto>(report);
        }

        public async Task<ReportedReviewDto> CreateAsync(ReportedReviewCreateDto dto)
        {
            var report = _mapper.Map<ReportedReview>(dto);
            _context.ReportedReviews.Add(report);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReportedReviewDto>(report);
        }

        public async Task<bool> UpdateAsync(Guid id, ReportedReviewUpdateDto dto)
        {
            var report = await _context.ReportedReviews.FindAsync(id);
            if (report == null) return false;

            report.ReportedAt = DateTime.UtcNow;
            report.Reason = dto.Reason;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var report = await _context.ReportedReviews.FindAsync(id);
            if (report == null) return false;

            _context.ReportedReviews.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
