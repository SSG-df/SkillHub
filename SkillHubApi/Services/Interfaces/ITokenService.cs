using System;

namespace SkillHubApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid userId, string username, string role);
    }
}
