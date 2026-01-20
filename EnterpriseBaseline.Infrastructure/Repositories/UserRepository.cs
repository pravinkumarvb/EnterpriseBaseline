using EnterpriseBaseline.Application.Interfaces.Repositories;
using EnterpriseBaseline.Domain.Entities;
using EnterpriseBaseline.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseBaseline.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByUserNameOrEmailAsync(string userNameOrEmail)
        {
            return await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u =>
                    u.UserName == userNameOrEmail ||
                    u.Email == userNameOrEmail);
        }
    }
}
