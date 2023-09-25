namespace HE.InvestmentLoans.Common.Exceptions;

public class NotFoundException : Exception
{
    public string EntityName { get; private set; }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
        EntityName = name;
    }

}
