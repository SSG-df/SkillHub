using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SkillHubApi.Services
{
    public class FileResourceService : IFileResourceService
    {
        private readonly SkillHubDbContext _context;

        public FileResourceService(SkillHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FileResourceDto>> GetAllAsync()
        {
            return await _context.FileResources
                .Select(f => new FileResourceDto
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    FileType = f.FileType
                })
                .ToListAsync();
        }

        public async Task<FileResourceDto?> GetByIdAsync(Guid id)
        {
            var file = await _context.FileResources.FindAsync(id);
            if (file == null) return null;

            return new FileResourceDto
            {
                Id = file.Id,
                FileName = file.FileName,
                FileType = file.FileType
            };
        }

        public async Task AddAsync(FileResourceCreateDto dto)
        {
            var file = new FileResource
            {
                FileName = dto.FileName,
                FileType = dto.FileType,
                LessonId = dto.LessonId
            };

            _context.FileResources.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FileResourceUpdateDto dto)
        {
            var file = await _context.FileResources.FindAsync(dto.Id);
            if (file == null) return;

            file.FileName = dto.FileName;
            file.FileType = dto.FileType;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var file = await _context.FileResources.FindAsync(id);
            if (file != null)
            {
                _context.FileResources.Remove(file);
                await _context.SaveChangesAsync();
            }
        }
    }
}
