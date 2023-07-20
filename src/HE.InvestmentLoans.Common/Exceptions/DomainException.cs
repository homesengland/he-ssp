namespace HE.InvestmentLoans.Common.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message, string errorCode, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    public string ErrorCode { get; }
}
