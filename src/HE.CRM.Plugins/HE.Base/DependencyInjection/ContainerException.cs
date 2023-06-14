using System;

namespace HE.Base.DependencyInjection
{
    public class ContainerException : Exception
    {
        public ContainerException(string message)
            : base(message)
        {
        }

        public ContainerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
