using EnterpriseBaseline.Application.DTOs.Auth;
using EnterpriseBaseline.Application.Interfaces.Repositories;
using EnterpriseBaseline.Application.Interfaces.Services;

namespace EnterpriseBaseline.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository
                .GetByUserNameOrEmailAsync(request.UserNameOrEmail);

            if (user == null || !user.IsActive)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Code)
                .Distinct();

            var accessToken =
                _jwtTokenGenerator.GenerateAccessToken(user, permissions);

            return new LoginResponseDto
            {
                AccessToken = accessToken
            };
        }
    }
}
