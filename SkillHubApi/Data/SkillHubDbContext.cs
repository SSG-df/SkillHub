using Microsoft.EntityFrameworkCore;
using SkillHubApi.Models;

namespace SkillHubApi.Data
{
    public class SkillHubDbContext : DbContext
    {
        public SkillHubDbContext(DbContextOptions<SkillHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<LessonEnrollment> LessonEnrollments => Set<LessonEnrollment>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<LessonTag> LessonTags => Set<LessonTag>();
        public DbSet<FileResource> FileResources => Set<FileResource>();
        public DbSet<ReportedReview> ReportedReviews => Set<ReportedReview>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<ReportRequest> ReportRequests => Set<ReportRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LessonTag>()
                .HasKey(lt => new { lt.LessonId, lt.TagId });

            modelBuilder.Entity<LessonTag>()
                .HasOne(lt => lt.Lesson)
                .WithMany(l => l.LessonTags)
                .HasForeignKey(lt => lt.LessonId);

            modelBuilder.Entity<LessonTag>()
                .HasOne(lt => lt.Tag)
                .WithMany(t => t.LessonTags)
                .HasForeignKey(lt => lt.TagId);
        }
    }
}
