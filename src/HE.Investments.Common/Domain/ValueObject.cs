namespace HE.Investments.Common.Domain;

public abstract class ValueObject
{
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
