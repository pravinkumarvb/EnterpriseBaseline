using EnterpriseBaseline.Domain.Entities;

namespace EnterpriseBaseline.Application.Interfaces.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(User user, IEnumerable<string> permissions);
        string GenerateRefreshToken();
    }
}
