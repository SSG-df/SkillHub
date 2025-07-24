using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace SkillHubApi.Data
{
    public static class SeedData
    {
        public static void Initialize(SkillHubDbContext context)
        {
            context.Database.Migrate();

            if (context.Users.Any()) return;

            var admin = new User
            {
                Username = "admin",
                Email = "admin@skillhub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), 
                Role = UserRole.Admin
            };

            var mentor = new User
            {
                Username = "mentor",
                Email = "mentor@skillhub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Mentor123"),
                Role = UserRole.Mentor,
                Bio = "Super puper mentor with 100 years experience in .NET"
            };

            var learner = new User
            {
                Username = "learner",
                Email = "learner@skillhub.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Learner123"),
                Role = UserRole.Learner,
                Bio = "Motivation podnyat",
            };

            context.Users.AddRange(admin, mentor, learner);
            context.SaveChanges();

            var tag1 = new Tag { Name = "C#" };
            var tag2 = new Tag { Name = "Web API" };
            var tag3 = new Tag { Name = "Entity Framework" };

            context.Tags.AddRange(tag1, tag2, tag3);
            context.SaveChanges();

            var lesson1 = new Lesson
            {
                Title = "Starting with .NET",
                Description = "Osnovy .NET va C#",
                Difficulty = DifficultyLevel.Beginner,
                Capacity = 10,
                StartTime = DateTime.UtcNow.AddDays(1),
                EndTime = DateTime.UtcNow.AddDays(2),
                MentorId = mentor.Id
            };

            var lesson2 = new Lesson
            {
                Title = "C# for chayninkov",
                Description = "Osnovy yazyka C#",
                Difficulty = DifficultyLevel.Intermediate,
                Capacity = 15,
                StartTime = DateTime.UtcNow.AddDays(3),
                EndTime = DateTime.UtcNow.AddDays(4),
                MentorId = mentor.Id
            };

            context.Lessons.AddRange(lesson1, lesson2);
            context.SaveChanges();

            var lessonTags = new[]
            {
                new LessonTag { LessonId = lesson1.Id, TagId = tag1.Id },
                new LessonTag { LessonId = lesson1.Id, TagId = tag2.Id },
                new LessonTag { LessonId = lesson2.Id, TagId = tag1.Id },
                new LessonTag { LessonId = lesson2.Id, TagId = tag3.Id }
            };

            context.LessonTags.AddRange(lessonTags);
            context.SaveChanges();

            var enrollment1 = new LessonEnrollment
            {
                LessonId = lesson1.Id,
                UserId = learner.Id,
                IsCompleted = true
            };

            var enrollment2 = new LessonEnrollment
            {
                LessonId = lesson2.Id,
                UserId = learner.Id,
                IsCompleted = false
            };

            context.LessonEnrollments.AddRange(enrollment1, enrollment2);
            context.SaveChanges();

            var attendance = new Attendance
            {
                EnrollmentId = enrollment1.Id,
                IsPresent = true,
                AttendedAt = DateTime.UtcNow
            };

            context.Attendances.Add(attendance);
            context.SaveChanges();

            var review = new Review
            {
                LessonId = lesson1.Id,
                UserId = learner.Id,
                Rating = 5,
                Comment = "Awesome lesson",
                IsVisible = true
            };

            context.Reviews.Add(review);
            context.SaveChanges();

            var reported = new ReportedReview
            {
                ReviewId = review.Id,
                Reason = "Spam "
            };

            context.ReportedReviews.Add(reported);
            context.SaveChanges();

            var file = new FileResource
            {
                LessonId = lesson1.Id,
                FileName = "lesson1.pdf",
                FileType = "uploads/lesson1.pdf"
            };

            context.FileResources.Add(file);
            context.SaveChanges();

            var reportRequest = new ReportRequest
            {
                RequestedById = admin.Id,
                Type = ReportType.LessonSummary,
                Format = "Formant"
            };

            context.ReportRequests.Add(reportRequest);
            context.SaveChanges();
        }
    }
}
