using SchoolManagementSystem.Domain.Entities;

namespace SchoolManagementSystem.Application.Common.Interfaces.Securities
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IEnumerable<KeyValuePair<string, string>>? extraClaims = null);
    }
}
