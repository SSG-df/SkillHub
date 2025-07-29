using System;
using System.ComponentModel.DataAnnotations;

namespace SkillHubApi.Dtos
{
    public class LessonEnrollmentCreateDto
    {
        [Required]
        public Guid LessonId { get; set; }
        [Required]
        public Guid UserId { get; set; }
    }

    public class LessonEnrollmentUpdateDto
    {
        public bool? IsCompleted { get; set; }
        public bool? IsCancelled { get; set; }
    }
}
