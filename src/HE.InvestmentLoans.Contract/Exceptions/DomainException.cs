namespace HE.InvestmentLoans.Contract.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message, string errorCode, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    public DomainException(string message, string errorCode, params (string Key, string Value)[] additionalData)
        : base(message)
    {
        ErrorCode = errorCode;

        AdditionalData = new Dictionary<string, string>();

        foreach (var (key, value) in additionalData)
        {
            AdditionalData.Add(key, value);
        }
    }

    public string ErrorCode { get; }

    public Dictionary<string, string> AdditionalData { get; private set; }
}
