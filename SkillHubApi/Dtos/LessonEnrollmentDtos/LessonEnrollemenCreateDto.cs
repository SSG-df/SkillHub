namespace SkillHubApi.Dtos
{
    public class LessonEnrollmentCreateDto
    { 
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
    }
}
