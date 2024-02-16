namespace HE.Investments.Common.Contract;

public record StringIdValueObject
{
    public StringIdValueObject(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Cannot create id for empty value");
        }

        Value = id;
    }

    public StringIdValueObject()
    {
        Value = string.Empty;
    }

    public string Value { get; }

    public bool IsNew => string.IsNullOrEmpty(Value);

    public override string ToString()
    {
        return Value;
    }
}
