using System;

namespace HE.CRM.Common.Api.Exceptions
{
    public abstract class ApiException : Exception
    {
        protected ApiException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
