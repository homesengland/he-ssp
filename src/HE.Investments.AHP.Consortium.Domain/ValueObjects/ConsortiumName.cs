using HE.Investments.Common.Domain;

namespace HE.Investments.AHP.Consortium.Domain.ValueObjects;

public class ConsortiumName : ValueObject
{
    public ConsortiumName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Consortium name cannot be empty.", nameof(value));
        }

        Value = value;
    }

    public string Value { get; }

    public static ConsortiumName GenerateName(string programmeName, string leadPartnerName)
    {
        if (string.IsNullOrWhiteSpace(programmeName))
        {
            throw new ArgumentException("Programme name cannot be empty.", nameof(programmeName));
        }

        if (string.IsNullOrWhiteSpace(leadPartnerName))
        {
            throw new ArgumentException("Lead partner name cannot be empty.", nameof(leadPartnerName));
        }

        return new ConsortiumName($"{programmeName} - {leadPartnerName}");
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
