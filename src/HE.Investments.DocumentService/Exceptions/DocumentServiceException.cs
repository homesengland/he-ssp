namespace HE.Investments.DocumentService.Exceptions;

public abstract class DocumentServiceException : Exception
{
    protected DocumentServiceException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
