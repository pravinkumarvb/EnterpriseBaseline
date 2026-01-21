using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBaseline.Application.Common
{
    public class ErrorResponse
    {
        public bool Success { get; set; } = false;
        public ErrorDetail Error { get; set; } = new();
        public string TraceId { get; set; } = string.Empty;
    }
}
