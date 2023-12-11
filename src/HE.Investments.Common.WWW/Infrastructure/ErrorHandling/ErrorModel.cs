namespace HE.Investments.Common.WWW.Infrastructure.ErrorHandling;

public class ErrorModel
{
    public ErrorModel()
    {
    }

    public ErrorModel(string message)
    {
        Message = message;
    }

    public ErrorModel(string message, string errorCode)
    {
        Message = message;
        ErrorCode = errorCode;
    }

    public ErrorModel(string message, string errorCode, Dictionary<string, object> additionalData)
    {
        Message = message;
        ErrorCode = errorCode;
        AdditionalData = additionalData;
    }

    public string Message { get; set; }

    public string Header { get; set; }

    public string Body { get; set; }

    public string ErrorCode { get; set; }

    public Dictionary<string, object> AdditionalData { get; set; }
}
