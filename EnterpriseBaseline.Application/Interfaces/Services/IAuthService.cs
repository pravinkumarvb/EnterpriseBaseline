using EnterpriseBaseline.Application.DTOs.Auth;

namespace EnterpriseBaseline.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
