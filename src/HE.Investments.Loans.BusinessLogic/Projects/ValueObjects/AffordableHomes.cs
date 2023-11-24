using HE.Investments.Common.Domain;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class AffordableHomes : ValueObject
{
    public AffordableHomes(string? value)
    {
        Value = value;
    }

    public string? Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value ?? null!;
    }
}
