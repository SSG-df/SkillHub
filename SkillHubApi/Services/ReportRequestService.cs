using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;

namespace SkillHubApi.Services
{
    public class ReportRequestService : IReportRequestService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;

        public ReportRequestService(SkillHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReportRequestDto>> GetAllAsync()
        {
            var reports = await _context.ReportRequests.ToListAsync();
            return _mapper.Map<IEnumerable<ReportRequestDto>>(reports);
        }

        public async Task<ReportRequestDto?> GetByIdAsync(Guid id)
        {
            var report = await _context.ReportRequests.FindAsync(id);
            return report == null ? null : _mapper.Map<ReportRequestDto>(report);
        }

        public async Task<ReportRequestDto> CreateAsync(ReportRequestCreateDto dto)
        {
            var report = _mapper.Map<ReportRequest>(dto);
            _context.ReportRequests.Add(report);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReportRequestDto>(report);
        }

        public async Task<bool> UpdateAsync(Guid id, ReportRequestUpdateDto dto)
        {
            var report = await _context.ReportRequests.FindAsync(id);
            if (report == null) return false;

            report.Reason = dto.Reason;

            _mapper.Map(dto, report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var report = await _context.ReportRequests.FindAsync(id);
            if (report == null) return false;

            _context.ReportRequests.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
