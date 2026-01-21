using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBaseline.Application.Exceptions
{
    public class UnauthorizedException : BusinessException
    {
        public UnauthorizedException(string message)
            : base("AUTH_UNAUTHORIZED", message)
        {
        }
    }
}
