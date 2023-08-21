namespace HE.InvestmentLoans.Common.Exceptions;

public class FileException : Exception
{
    public FileException(string? message)
        : base(message)
    {
    }
}
