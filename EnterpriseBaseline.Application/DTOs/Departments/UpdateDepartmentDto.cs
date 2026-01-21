using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBaseline.Application.DTOs.Departments
{
    public class UpdateDepartmentDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
