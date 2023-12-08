namespace HE.Investments.Common.Domain.ValueObjects;

public class StringIdValueObject : ValueObject
{
    protected StringIdValueObject(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Cannot create id for empty value");
        }

        Value = id;
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
