using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHubApi.Dtos
{
    public class AttendanceCreateDto
    {
        [Required]
        public Guid LessonEnrollmentId { get; set; }
        [Required]
        public DateTime Date { get; set; } 
        public bool IsPresent { get; set; } = true;
    }
}