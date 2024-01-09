using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class IsAdditionalPaymentRequested : ValueObject, IQuestion
{
    public IsAdditionalPaymentRequested(bool isRequested)
    {
        IsRequested = isRequested;
    }

    public bool IsRequested { get; }

    public bool IsAnswered()
    {
        return true;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsRequested;
    }
}
