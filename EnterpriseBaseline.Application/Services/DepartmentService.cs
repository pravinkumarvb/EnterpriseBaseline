using EnterpriseBaseline.Application.DTOs.Departments;
using EnterpriseBaseline.Application.Exceptions;
using EnterpriseBaseline.Application.Interfaces.Repositories;
using EnterpriseBaseline.Application.Interfaces.Services;
using EnterpriseBaseline.Domain.Entities;

namespace EnterpriseBaseline.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();

            return departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description
            }).ToList();
        }

        public async Task<DepartmentDto?> GetByIdAsync(int id)
        {
           var department= await _repository.GetByIdAsync(id);
            if(department==null)
            {
                return null;
            }
            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }

        public async Task<int> CreateAsync(CreateDepartmentDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new ValidationException("Department name already exists");

            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _repository.AddAsync(department);
            return department.Id;
        }

        public async Task UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _repository.GetByIdAsync(id)
                ?? throw new ValidationException("Department not found");

            if (await _repository.ExistsByNameAsync(dto.Name, id))
                throw new ValidationException("Department name already exists");

            department.Name = dto.Name;
            department.Description = dto.Description;

            await _repository.UpdateAsync(department);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await _repository.GetByIdAsync(id)
                ?? throw new ValidationException("Department not found");

            await _repository.DeleteAsync(department);
        }
    }
}
