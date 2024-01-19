namespace HE.Investments.Common.Domain;

public abstract class ValueObject
{
#pragma warning disable S3875 // This is value object, so it is ok to override == operator on reference types
    public static bool operator ==(ValueObject? a, ValueObject? b)
#pragma warning restore S3875
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var valueObject = (ValueObject)obj;
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }

    public override int GetHashCode()
    {
        return GetAtomicValues().Aggregate(1, HashCode.Combine);
    }

    protected abstract IEnumerable<object?> GetAtomicValues();
}
