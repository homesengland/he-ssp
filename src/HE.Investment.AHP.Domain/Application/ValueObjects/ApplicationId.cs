using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationId : ValueObject
{
    public ApplicationId(string id)
    {
        Value = Guard.Argument(id, nameof(id)).NotEmpty().NotWhiteSpace();
    }

    public string Value { get; }

    public static ApplicationId Empty() => new(Guid.Empty.ToString());

    public override string ToString()
    {
        return Value;
    }

    public bool IsEmpty() => Value.Equals(Guid.Empty.ToString(), StringComparison.Ordinal);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
