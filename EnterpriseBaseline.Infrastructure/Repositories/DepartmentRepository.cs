using EnterpriseBaseline.Application.Interfaces.Repositories;
using EnterpriseBaseline.Domain.Entities;
using EnterpriseBaseline.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseBaseline.Infrastructure.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _dbContext;

        public DepartmentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Department>> GetAllAsync()
        {
            return await _dbContext.Departments.ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _dbContext.Departments.FindAsync(id);
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        {
            return await _dbContext.Departments.AnyAsync(d =>
                d.Name == name &&
                (!excludeId.HasValue || d.Id != excludeId.Value));
        }

        public async Task AddAsync(Department department)
        {
            _dbContext.Departments.Add(department);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _dbContext.Departments.Update(department);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department department)
        {
            department.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }
    }
}
