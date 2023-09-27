namespace HE.InvestmentLoans.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
        EntityName = name;
    }

    public string EntityName { get; private set; }
}
