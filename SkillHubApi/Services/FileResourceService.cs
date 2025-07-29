using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace SkillHubApi.Services
{
    public class FileResourceService : IFileResourceService
    {
        private readonly SkillHubDbContext _context;
        private readonly string _uploadDirectory = "Uploads";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileResourceService(SkillHubDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            Directory.CreateDirectory(_uploadDirectory);
        }

        public async Task<IEnumerable<FileResourceDto>> GetAllAsync()
        {
            return await _context.FileResources
                .Select(f => new FileResourceDto
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    LessonId = f.LessonId,
                    CreatedBy = f.CreatedBy,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<FileResourceDto?> GetByIdAsync(Guid id)
        {
            var file = await _context.FileResources.FindAsync(id);
            return file == null ? null : new FileResourceDto
            {
                Id = file.Id,
                FileName = file.FileName,
                FileType = file.FileType,
                LessonId = file.LessonId,
                CreatedBy = file.CreatedBy,
                CreatedAt = file.CreatedAt
            };
        }

        public async Task AddAsync(FileResourceCreateDto dto)
        {
            var currentUserId = GetCurrentUserId();

            var file = new FileResource
            {
                FileName = dto.FileName,
                FileType = dto.FileType,
                LessonId = dto.LessonId,
                StoragePath = string.Empty,
                CreatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.FileResources.Add(file);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FileResourceUpdateDto dto)
        {
            var file = await _context.FileResources.FindAsync(dto.Id);
            if (file != null)
            {
                file.FileName = dto.FileName;
                file.FileType = dto.FileType;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var file = await _context.FileResources.FindAsync(id);
            if (file != null)
            {
                if (!string.IsNullOrEmpty(file.StoragePath))
                {
                    File.Delete(file.StoragePath);
                }
                _context.FileResources.Remove(file);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UploadFileAsync(Guid fileResourceId, IFormFile file)
        {
            var fileResource = await _context.FileResources.FindAsync(fileResourceId);
            if (fileResource == null) 
                throw new ArgumentException("File resource not found");

            var filePath = Path.Combine(_uploadDirectory, $"{fileResourceId}_{file.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            fileResource.StoragePath = filePath;
            fileResource.FileName = file.FileName;
            fileResource.FileType = file.ContentType;
            await _context.SaveChangesAsync();
        }

        public async Task<FileDownloadDto> DownloadFileAsync(Guid id)
        {
            var fileResource = await _context.FileResources.FindAsync(id);
            if (fileResource == null || !File.Exists(fileResource.StoragePath))
                throw new FileNotFoundException();

            return new FileDownloadDto
            {
                Content = await File.ReadAllBytesAsync(fileResource.StoragePath),
                FileName = fileResource.FileName,
                ContentType = fileResource.FileType
            };
        }

        public async Task<IEnumerable<FileResourceDto>> GetByLessonIdAsync(Guid lessonId)
        {
            return await _context.FileResources
                .Where(f => f.LessonId == lessonId)
                .Select(f => new FileResourceDto
                {
                    Id = f.Id,
                    FileName = f.FileName,
                    FileType = f.FileType,
                    LessonId = f.LessonId,
                    CreatedBy = f.CreatedBy,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync();
        }

        private Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out var result))
            {
                return result;
            }
            throw new UnauthorizedAccessException("Invalid user ID");
        }
    }
}