namespace SkillHubApi.Dtos
{
    public class LessonEnrollmentDto
    {
        public Guid Id { get; set; }   
        public Guid LessonId { get; set; }
        public Guid UserId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
