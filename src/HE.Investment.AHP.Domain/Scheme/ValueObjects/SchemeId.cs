using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class SchemeId : ValueObject
{
    public SchemeId(string id)
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
