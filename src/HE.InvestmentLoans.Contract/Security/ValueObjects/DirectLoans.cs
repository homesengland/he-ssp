using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Security.ValueObjects;

public class DirectLoans : ValueObject
{
    public DirectLoans(bool exists)
    {
        Exists = exists;
    }

    public DirectLoans(bool exists, bool? canBeSubordinate, string reasonWhyCannotBeSubordinated)
    {
        Exists = exists;

        if (Exists)
        {
            CanBeSubordinated = canBeSubordinate;
        }

        if (CanBeSubordinated.HasValue && !CanBeSubordinated.Value)
        {
            ReasonWhyCannotBeSubordinated = reasonWhyCannotBeSubordinated;
        }
    }

    public bool Exists { get; }

    public bool? CanBeSubordinated { get; }

    public string ReasonWhyCannotBeSubordinated { get; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Exists;
        yield return CanBeSubordinated;
        yield return ReasonWhyCannotBeSubordinated;
    }
}
