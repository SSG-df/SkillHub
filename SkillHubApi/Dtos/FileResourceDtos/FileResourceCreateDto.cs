using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHubApi.Dtos
{
    public class FileResourceCreateDto
    {
        [Required]
        public string FileName { get; set; } = string.Empty;
        [Required]
        public string FileType { get; set; } = string.Empty;
        [Required]
        public Guid LessonId { get; set; }
    }
}