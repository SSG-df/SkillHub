using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SkillHubApi.Services
{
    public class LessonTagService : ILessonTagService
    {
        private readonly SkillHubDbContext _context;

        public LessonTagService(SkillHubDbContext context)
        {
            _context = context;
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
                        Name = lt.Tag.Name
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
                    Name = lessonTag.Tag.Name
                }
            };
        }

        public async Task<LessonTagDto> CreateAsync(LessonTagCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TagName))
                throw new ArgumentException("TagName is required");

            // ищем существующий тег
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == dto.TagName.ToLower());

            // если не существует — создаём
            if (tag == null)
            {
                tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = dto.TagName
                };
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync(); // обязательно сохранить, чтобы получить Id
            }

            // проверка на дубликат
            var exists = await _context.LessonTags
                .AnyAsync(lt => lt.LessonId == dto.LessonId && lt.TagId == tag.Id);
            if (exists)
                throw new InvalidOperationException("This tag is already linked to the lesson.");

            // создаём LessonTag
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
                    Name = tag.Name
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
