using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBaseline.Application.Exceptions
{
    public class ValidationException : BusinessException
    {
        public ValidationException(string message, object? details = null)
            : base("VALIDATION_FAILED", message)
        {
            Details = details;
        }

        public object? Details { get; }
    }
}
