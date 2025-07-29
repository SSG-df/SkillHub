using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHubApi.Dtos
{
    public class FileResourceUpdateDto
    {
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        public string FileType { get; set; } = string.Empty;
    }
}