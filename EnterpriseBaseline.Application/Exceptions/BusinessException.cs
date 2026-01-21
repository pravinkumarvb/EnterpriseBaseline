using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseBaseline.Application.Exceptions
{
    public abstract class BusinessException : Exception
    {
        public string Code { get; }

        protected BusinessException(string code, string message)
            : base(message)
        {
            Code = code;
        }
    }
}
