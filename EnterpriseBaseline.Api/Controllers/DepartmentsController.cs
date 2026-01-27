using EnterpriseBaseline.Application.Common;
using EnterpriseBaseline.Application.DTOs.Departments;
using EnterpriseBaseline.Application.Exceptions;
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

        [HttpGet("{id}")]
        [Authorize(Policy = "Departments.View")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _service.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound(ApiResponse<string>.Fail(
                    "Department not found", HttpContext.TraceIdentifier));
            }

            return Ok(ApiResponse<DepartmentDto>.Ok(
                department, "Department fetched", HttpContext.TraceIdentifier));
        }

        [HttpPost]
        [Authorize(Policy = "Departments.Create")]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
        {
            try
            {
                var id = await _service.CreateAsync(dto);

                return Ok(ApiResponse<int>.Ok(
                    id, "Department created", HttpContext.TraceIdentifier));
            }
            catch (ValidationException ex)
            {
                return Conflict(ApiResponse<string>.Fail(
                    ex.Message, HttpContext.TraceIdentifier));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Departments.Update")]
        public async Task<IActionResult> Update(int id, UpdateDepartmentDto dto)
        {
            try
            {
                await _service.UpdateAsync(id, dto);

                return Ok(ApiResponse<string>.Ok(
                    "OK", "Department updated", HttpContext.TraceIdentifier));
            }
            catch (ValidationException ex)
            {
                // e.g. duplicate department name
                return Conflict(ApiResponse<string>.Fail(
                    ex.Message, HttpContext.TraceIdentifier));
            }
            catch (KeyNotFoundException)
            {
                // department does not exist
                return NotFound(ApiResponse<string>.Fail(
                    "Department not found", HttpContext.TraceIdentifier));
            }
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
