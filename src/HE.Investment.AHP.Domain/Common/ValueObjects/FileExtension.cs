using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Common.ValueObjects;

public class FileExtension : ValueObject
{
    public FileExtension(string? value)
    {
        Value = value?.Trim().ToLowerInvariant() ?? string.Empty;
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
