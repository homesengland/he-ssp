namespace HE.Investments.Common.CRM.Exceptions;

public sealed class CrmApiSerializationException : CrmException
{
    public CrmApiSerializationException(Exception? innerException = null, string? responseContent = null)
        : base("Serialization error occurred while parsing CRM API response.", innerException)
    {
        ResponseContent = responseContent;
    }

    public string? ResponseContent { get; }
}
