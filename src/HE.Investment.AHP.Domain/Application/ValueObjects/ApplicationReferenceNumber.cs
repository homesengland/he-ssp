using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Application.ValueObjects;

public class ApplicationReferenceNumber : ValueObject
{
    public ApplicationReferenceNumber(string? value)
    {
        Value = value;
    }

    public string? Value { get; private set; }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
