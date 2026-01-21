using EnterpriseBaseline.Application.Common;
using EnterpriseBaseline.Application.DTOs.Departments;
using EnterpriseBaseline.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseBaseline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;

        public DepartmentsController(IDepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Policy = "Departments.View")]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse<List<DepartmentDto>>.Ok(
                data, "Departments fetched", HttpContext.TraceIdentifier));
        }

        [HttpPost]
        [Authorize(Policy = "Departments.Create")]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return Ok(ApiResponse<int>.Ok(
                id, "Department created", HttpContext.TraceIdentifier));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Departments.Update")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok(ApiResponse<string>.Ok(
                "OK", "Department updated", HttpContext.TraceIdentifier));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Departments.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.Ok(
                "OK", "Department deleted", HttpContext.TraceIdentifier));
        }
    }
}
