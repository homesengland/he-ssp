namespace HE.Investments.DocumentService.Exceptions;

public class DocumentServiceSerializationException : DocumentServiceException
{
    public DocumentServiceSerializationException(string response, Exception? innerException = null)
        : base("The document service request result is invalid.", innerException)
    {
        Response = response;
    }

    public string Response { get; }
}
