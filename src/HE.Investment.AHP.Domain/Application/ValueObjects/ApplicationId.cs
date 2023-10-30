using Dawn;
using HE.InvestmentLoans.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationId : ValueObject
{
    public ApplicationId(string id)
    {
        Value = Guard.Argument(id, nameof(id)).NotEmpty().NotWhiteSpace();
    }

    public string Value { get; }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
