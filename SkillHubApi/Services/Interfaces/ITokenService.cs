using System;

namespace SkillHubApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string username, string role);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
    }
}