using SkillHubApi.Dtos;

namespace SkillHubApi.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllAsync();
        Task<TagDto?> GetByIdAsync(Guid id);
        Task<TagDto> CreateAsync(TagCreateDto tagCreateDto);
        Task<bool> UpdateAsync(Guid id, TagUpdateDto tagUpdateDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
