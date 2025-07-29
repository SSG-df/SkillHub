using SkillHubApi.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHubApi.Services
{
    public interface IFileResourceService
    {
        Task<IEnumerable<FileResourceDto>> GetAllAsync();
        Task<FileResourceDto?> GetByIdAsync(Guid id);
        Task AddAsync(FileResourceCreateDto dto);
        Task UpdateAsync(FileResourceUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task UploadFileAsync(Guid fileResourceId, IFormFile file);
        Task<FileDownloadDto> DownloadFileAsync(Guid id);
        Task<IEnumerable<FileResourceDto>> GetByLessonIdAsync(Guid lessonId);
    }

    public class FileDownloadDto
    {
        public byte[]? Content { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
}