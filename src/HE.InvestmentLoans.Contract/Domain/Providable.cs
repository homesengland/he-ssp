using System.Diagnostics.CodeAnalysis;
using HE.InvestmentLoans.Contract.Exceptions;

namespace HE.InvestmentLoans.Contract.Domain;

[SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Allowed for core classes")]
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Allowed for core classes")]
public class Providable<T> : ValueObject
    where T : ValueObject
{
    private readonly T? _value;

    public Providable(T valueObject)
    {
        _value = valueObject;
        IsProvided = true;
    }

    protected Providable()
    {
        IsProvided = false;
    }

    public bool IsProvided { get; }

    public bool IsNotProvided => IsProvided is false;

    public T Value
    {
        get
        {
            if (IsNotProvided)
            {
                throw new DomainException($"Value of {nameof(T)} has not be provided", CommonErrorCodes.ValueWasNotProvided);
            }

            return _value!;
        }
    }

    public static Providable<T> NotProvided() => new();

    public static Providable<T> Provided(T valueObject) => new(valueObject);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsProvided;
        yield return _value;
    }
}
