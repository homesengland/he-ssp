namespace HE.Investments.Api.Exceptions;

public sealed class ApiSerializationException : ApiException
{
    public ApiSerializationException(Exception? innerException = null, string? responseContent = null)
        : base("Serialization error occurred while parsing CRM API response.", innerException)
    {
        ResponseContent = responseContent;
    }

    public string? ResponseContent { get; }
}
