using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;

namespace SkillHubApi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(SkillHubDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto?> GetByIdAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            return review == null ? null : _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetByLessonIdAsync(Guid lessonId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.LessonId == lessonId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto> CreateAsync(ReviewCreateDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return _mapper.Map<ReviewDto>(review);
        }

        public async Task<bool> UpdateAsync(Guid id, ReviewUpdateDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _mapper.Map(reviewDto, review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
