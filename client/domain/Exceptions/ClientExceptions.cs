using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Exceptions
{
    public class ClientNotFoundException : Exception
    {
        public ClientNotFoundException(string message) : base(message)
        {

        }
    }

    public class ClientBadRequestException : Exception
    {
        public ClientBadRequestException(string message) : base(message)
        {

        }
    }


    public class ClientUnauthorizedException : Exception
    {
        public ClientUnauthorizedException(string message) : base(message)
        {

        }
    }
}
