using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SkillHubApi.Services
{
    public class LessonTagService : ILessonTagService
    {
        private readonly SkillHubDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LessonTagService(SkillHubDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private Guid GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var result) ? result : Guid.Empty;
        }

        public async Task<IEnumerable<LessonTagDto>> GetAllAsync()
        {
            return await _context.LessonTags
                .Include(lt => lt.Tag)
                .Select(lt => new LessonTagDto
                {
                    LessonId = lt.LessonId,
                    TagId = lt.TagId,
                    CreatedAt = lt.CreatedAt,
                    Tag = new TagDto
                    {
                        Id = lt.Tag.Id,
                        Name = lt.Tag.Name,
                        CreatedBy = lt.Tag.CreatedBy
                    }
                })
                .ToListAsync();
        }

        public async Task<LessonTagDto?> GetByIdAsync(Guid lessonId, Guid tagId)
        {
            var lessonTag = await _context.LessonTags
                .Include(lt => lt.Tag)
                .FirstOrDefaultAsync(lt => lt.LessonId == lessonId && lt.TagId == tagId);

            if (lessonTag == null) return null;

            return new LessonTagDto
            {
                LessonId = lessonTag.LessonId,
                TagId = lessonTag.TagId,
                CreatedAt = lessonTag.CreatedAt,
                Tag = new TagDto
                {
                    Id = lessonTag.Tag.Id,
                    Name = lessonTag.Tag.Name,
                    CreatedBy = lessonTag.Tag.CreatedBy
                }
            };
        }

        public async Task<LessonTagDto> CreateAsync(LessonTagCreateDto dto)
        {
            var currentUserId = GetCurrentUserId();
            if (currentUserId == Guid.Empty)
                throw new UnauthorizedAccessException("User not authenticated");

            if (string.IsNullOrWhiteSpace(dto.TagName))
                throw new ArgumentException("TagName is required");

            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == dto.TagName.ToLower());

            if (tag == null)
            {
                tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = dto.TagName,
                    CreatedBy = currentUserId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            var exists = await _context.LessonTags
                .AnyAsync(lt => lt.LessonId == dto.LessonId && lt.TagId == tag.Id);
            if (exists)
                throw new InvalidOperationException("This tag is already linked to the lesson.");

            var lessonTag = new LessonTag
            {
                LessonId = dto.LessonId,
                TagId = tag.Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.LessonTags.Add(lessonTag);
            await _context.SaveChangesAsync();

            return new LessonTagDto
            {
                LessonId = lessonTag.LessonId,
                TagId = lessonTag.TagId,
                CreatedAt = lessonTag.CreatedAt,
                Tag = new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    CreatedBy = tag.CreatedBy
                }
            };
        }

        public async Task<bool> DeleteAsync(Guid lessonId, Guid tagId)
        {
            var lessonTag = await _context.LessonTags
                .FirstOrDefaultAsync(lt => lt.LessonId == lessonId && lt.TagId == tagId);

            if (lessonTag == null)
                return false;

            _context.LessonTags.Remove(lessonTag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}