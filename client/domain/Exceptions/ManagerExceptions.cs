using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Exceptions
{
    public class ManagerNotFoundException : Exception
    {
        public ManagerNotFoundException(string message) : base(message)
        {

        }
    }

    public class ManagerBadRequestException : Exception
    {
        public ManagerBadRequestException(string message) : base(message)
        {

        }
    }
}
