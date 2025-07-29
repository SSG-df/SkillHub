using Microsoft.EntityFrameworkCore;
using SkillHubApi.Data;
using SkillHubApi.Dtos;
using SkillHubApi.Interfaces;
using SkillHubApi.Models;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace SkillHubApi.Services
{
    public class UserService : IUserService
    {
        private readonly SkillHubDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            SkillHubDbContext context, 
            ITokenService tokenService, 
            ILogger<UserService> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _logger = logger;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            return user == null ? null : MapToUserDto(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user == null ? null : MapToUserDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20)
        {
            return await _context.Users
                .OrderBy(u => u.Username)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => MapToUserDto(u))
                .ToListAsync();
        }

        public async Task<UserDto> CreateAsync(UserCreateDto userCreateDto)
        {
            if (!IsValidEmail(userCreateDto.Email))
                throw new ArgumentException("Invalid email format");

            var user = new User
            {
                Username = userCreateDto.Username,
                Email = userCreateDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password),
                Role = userCreateDto.Role,
                Bio = userCreateDto.Bio,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return MapToUserDto(user);
        }

        public async Task<bool> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(userUpdateDto.Id);
            if (user == null) return false;

            if (userUpdateDto.Email != null && !IsValidEmail(userUpdateDto.Email))
                throw new ArgumentException("Invalid email format");

            user.Email = userUpdateDto.Email ?? user.Email;
            user.Bio = userUpdateDto.Bio ?? user.Bio;
            user.Role = userUpdateDto.Role;

            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ActivateAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = true;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeactivateAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash)) 
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
                return new AuthResponseDto { Success = false, ErrorMessage = "Username already exists" };

            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return new AuthResponseDto { Success = false, ErrorMessage = "Email already exists" };

            if (!IsValidEmail(registerDto.Email))
                return new AuthResponseDto { Success = false, ErrorMessage = "Invalid email format" };

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Username, user.Role.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Token = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddDays(1),
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return new AuthResponseDto { Success = false, ErrorMessage = "Invalid credentials" };

            if (!user.IsActive)
                return new AuthResponseDto { Success = false, ErrorMessage = "Account is deactivated" };

            var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Username, user.Role.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Token = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddDays(1),
                User = MapToUserDto(user)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken && 
                                      u.RefreshTokenExpiry > DateTime.UtcNow);

            if (user == null)
                return new AuthResponseDto { Success = false, ErrorMessage = "Invalid refresh token" };

            var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.Username, user.Role.ToString());
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddDays(1),
                User = MapToUserDto(user)
            };
        }

        public async Task<bool> CheckPasswordAsync(Guid userId, string password)
        {
            var user = await _context.Users.FindAsync(userId);
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Bio = user.Bio,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}