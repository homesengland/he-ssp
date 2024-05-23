namespace HE.Investments.Common.CRM.Exceptions;

public sealed class CrmException : Exception
{
    public CrmException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
