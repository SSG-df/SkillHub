using SkillHubApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHubApi.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAsync();
        Task<AttendanceDto?> GetByIdAsync(Guid id);
        Task<AttendanceDto> AddAsync(AttendanceCreateDto dto);
        Task<bool> UpdateAsync(Guid id, AttendanceUpdateDto dto);
        Task<bool> DeleteAsync(Guid id);
        
        Task<bool> VerifyAccessAsync(Guid attendanceId, Guid userId);
    }
}