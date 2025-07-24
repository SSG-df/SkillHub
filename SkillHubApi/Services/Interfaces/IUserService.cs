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
        Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
        Task<bool> CreateAsync(UserCreateDto userCreateDto);
        Task<bool> UpdateAsync(UserUpdateDto userUpdateDto);
        Task<bool> ActivateAsync(Guid userId);
        Task<bool> DeactivateAsync(Guid userId);

        Task<bool> CheckPasswordAsync(Guid userId, string password);
    }
}
