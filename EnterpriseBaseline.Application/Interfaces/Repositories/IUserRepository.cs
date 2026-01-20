using EnterpriseBaseline.Domain.Entities;

namespace EnterpriseBaseline.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail);
    }
}
