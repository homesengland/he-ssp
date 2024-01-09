namespace HE.Investments.Common.Contract.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message, string errorCode, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        AdditionalData = new Dictionary<string, object>();
    }

    public DomainException(string message, string errorCode, params (string Key, object Value)[] additionalData)
        : base(message)
    {
        ErrorCode = errorCode;

        AdditionalData = new Dictionary<string, object>();

        foreach (var (key, value) in additionalData)
        {
            AdditionalData.Add(key, value);
        }
    }

    public string ErrorCode { get; }

    public Dictionary<string, object> AdditionalData { get; private set; }
}
