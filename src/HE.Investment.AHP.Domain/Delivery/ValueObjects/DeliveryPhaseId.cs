using Dawn;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Delivery.ValueObjects;

public class DeliveryPhaseId : ValueObject
{
    public DeliveryPhaseId(string id)
    {
        Value = Guard.Argument(id, nameof(id)).NotEmpty().NotWhiteSpace();
    }

    private DeliveryPhaseId()
    {
        Value = string.Empty;
    }

    public string Value { get; }

    public bool IsNew => string.IsNullOrEmpty(Value);

    public static DeliveryPhaseId New() => new();

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
