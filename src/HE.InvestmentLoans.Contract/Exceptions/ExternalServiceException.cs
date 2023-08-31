namespace HE.InvestmentLoans.Contract.Exceptions;
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
