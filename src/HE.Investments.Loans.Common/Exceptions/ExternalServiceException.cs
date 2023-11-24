namespace HE.Investments.Loans.Common.Exceptions;
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
