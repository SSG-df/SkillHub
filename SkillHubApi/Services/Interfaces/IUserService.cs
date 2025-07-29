using SkillHubApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillHubApi.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(Guid id);
        Task<UserDto?> GetByUsernameAsync(string username);
        Task<IEnumerable<UserDto>> GetAllAsync(int pageNumber = 1, int pageSize = 20);
        Task<UserDto> CreateAsync(UserCreateDto userCreateDto);
        Task<bool> UpdateAsync(UserUpdateDto userUpdateDto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ActivateAsync(Guid userId);
        Task<bool> DeactivateAsync(Guid userId);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
        Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
        Task<bool> CheckPasswordAsync(Guid userId, string password);
    }
}