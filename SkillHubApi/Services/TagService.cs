using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;

namespace SkillHubApi.Services
{
    public class TagService : ITagService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;

        public TagService(SkillHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            var tags = await _context.Tags.ToListAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto?> GetByIdAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            return tag == null ? null : _mapper.Map<TagDto>(tag);
        }

        public async Task<TagDto> CreateAsync(TagCreateDto tagCreateDto)
        {
            var tag = _mapper.Map<Tag>(tagCreateDto);
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return _mapper.Map<TagDto>(tag);
        }

        public async Task<bool> UpdateAsync(Guid id, TagUpdateDto tagUpdateDto)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _mapper.Map(tagUpdateDto, tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
