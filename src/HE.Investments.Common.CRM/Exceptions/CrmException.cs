namespace HE.Investments.Common.CRM.Exceptions;

public class CrmException : Exception
{
    public CrmException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
