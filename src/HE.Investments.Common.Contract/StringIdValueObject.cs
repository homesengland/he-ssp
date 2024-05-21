namespace HE.Investments.Common.Contract;

public abstract record StringIdValueObject
{
    protected StringIdValueObject(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Cannot create id for empty value");
        }

        Value = value;
    }

    protected StringIdValueObject()
    {
        Value = string.Empty;
    }

    public string Value { get; }

    public bool IsNew => string.IsNullOrEmpty(Value);

    public static string FromShortGuidAsStringToGuidAsString(string value) => ShortGuid.ToGuidAsString(value);

    public static string FromGuidToShortGuidAsString(Guid value) => ShortGuid.FromGuid(value).Value;

    public static string FromStringToShortGuidAsString(string value) => ShortGuid.FromString(value).Value;

    public virtual bool Equals(StringIdValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ShortGuid.TryToGuidAsString(Value) == ShortGuid.TryToGuidAsString(other.Value);
    }

    public override int GetHashCode()
    {
        return ShortGuid.TryToGuidAsString(Value).GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}
