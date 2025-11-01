using System;

namespace API.Domain.Exceptions
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message) : base(message)
        {
            
        }
    }

    public class DomainBadRequestException : Exception
    {
        public DomainBadRequestException(string message) : base(message)
        {

        }
    }
}
