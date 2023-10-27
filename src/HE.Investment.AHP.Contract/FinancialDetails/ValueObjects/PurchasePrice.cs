using Dawn;
using HE.Investment.AHP.Contract.Domain;

namespace HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
public class PurchasePrice : ValueObject
{
    public PurchasePrice(string value, bool isFinal)
    {
        Value = value;
        IsFinal = isFinal;
    }

    public string Value { get; }

    public bool IsFinal { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return IsFinal;
    }
}
