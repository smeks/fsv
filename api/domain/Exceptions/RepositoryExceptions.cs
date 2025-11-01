using System;

namespace API.Domain.Exceptions
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException(string message) : base(message)
        {
            
        }
    }

    public class RepositoryBadRequestException : Exception
    {
        public RepositoryBadRequestException(string message) : base(message)
        {

        }
    }
}
