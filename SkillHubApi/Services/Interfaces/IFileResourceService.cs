using SkillHubApi.Dtos;

namespace SkillHubApi.Services
{
    public interface IFileResourceService
    {
        Task<IEnumerable<FileResourceDto>> GetAllAsync();
        Task<FileResourceDto?> GetByIdAsync(Guid id);
        Task AddAsync(FileResourceCreateDto dto);
        Task UpdateAsync(FileResourceUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
