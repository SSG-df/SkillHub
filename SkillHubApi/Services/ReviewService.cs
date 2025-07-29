using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SkillHubApi.Services
{
    public class ReviewService : IReviewService
    {
        private readonly SkillHubDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewService(
            SkillHubDbContext context, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReviewDto> CreateAsync(ReviewCreateDto reviewDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId)) 
                throw new UnauthorizedAccessException("User ID not found in token");

            if (!Guid.TryParse(userId, out var createdBy))
                throw new UnauthorizedAccessException("Invalid user ID format");

            var review = new Review
            {
                Rating = reviewDto.Rating,
                LessonId = reviewDto.LessonId,
                Comment = reviewDto.Comment,
                CreatedBy = createdBy, 
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return new ReviewDto
            {
                Id = review.Id,
                Comment = review.Comment,
                Rating = review.Rating,
                LessonId = review.LessonId,
                CreatedBy = review.CreatedBy,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<ReviewDto?> GetByIdAsync(Guid id)
        {
            var review = await _context.Reviews
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            return review == null ? null : _mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetByLessonIdAsync(Guid lessonId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.LessonId == lessonId)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        public async Task<bool> UpdateAsync(Guid id, ReviewUpdateDto reviewDto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            var currentUserId = GetCurrentUserId();
            if (!HasEditPermission(review, currentUserId))
                return false;

            _mapper.Map(reviewDto, review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            var currentUserId = GetCurrentUserId();
            if (!HasDeletePermission(review, currentUserId))
                return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        private bool IsAdmin()
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
        }

        private bool HasEditPermission(Review review, Guid currentUserId)
        {
            return review.CreatedBy == currentUserId || IsAdmin();
        }

        private bool HasDeletePermission(Review review, Guid currentUserId)
        {
            return review.CreatedBy == currentUserId || IsAdmin();
        }
    }
}