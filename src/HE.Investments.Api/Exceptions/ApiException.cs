namespace HE.Investments.Api.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
