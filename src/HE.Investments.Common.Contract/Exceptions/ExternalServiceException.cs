namespace HE.Investments.Common.Contract.Exceptions;

public class ExternalServiceException : Exception
{
    public ExternalServiceException(string message)
        : base(message)
    {
    }

    public ExternalServiceException()
    {
    }
}
