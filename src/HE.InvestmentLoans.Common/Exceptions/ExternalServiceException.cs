namespace HE.InvestmentLoans.Common.Exceptions;
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
