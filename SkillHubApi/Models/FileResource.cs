using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillHubApi.Models
{
    public class FileResource
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string FileName { get; set; } = string.Empty;
        [Required]
        public string FileType { get; set; } = string.Empty;
        public string StoragePath { get; set; } = string.Empty;
        [Required]
        public Guid LessonId { get; set; }
        [ForeignKey("LessonId")]
        public Lesson? Lesson { get; set; }
        [Required]
        public Guid CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}