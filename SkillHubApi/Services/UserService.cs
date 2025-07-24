using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Interfaces;
using SkillHubApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace SkillHubApi.Services
{
    public class UserService : IUserService
    {
        private readonly SkillHubDbContext _context;

        public UserService(SkillHubDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return MapToUserDto(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            return MapToUserDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            var users = await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users.Select(u => MapToUserDto(u));
        }

        public async Task<bool> CreateAsync(UserCreateDto userCreateDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userCreateDto.Username || u.Email == userCreateDto.Email))
                return false;

            var user = new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password),
                Role = userCreateDto.Role,
                Bio = userCreateDto.Bio,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(userUpdateDto.Id);
            if (user == null) return false;

            user.Email = userUpdateDto.Email;
            user.Bio = userUpdateDto.Bio;
            user.Role = userUpdateDto.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Bio = user.Bio,
                IsActive = user.IsActive
            };
        }
        private readonly ITokenService _tokenService;

        public UserService(SkillHubDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return null;

            var token = _tokenService.GenerateToken(user.Id, user.Username, user.Role.ToString());

            return new AuthResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Bio = user.Bio,
                    IsActive = user.IsActive
                }
            };
        }
    }
}
