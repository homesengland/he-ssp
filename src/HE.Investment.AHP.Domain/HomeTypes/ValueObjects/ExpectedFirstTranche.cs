using System.Globalization;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class ExpectedFirstTranche : ValueObject
{
    public ExpectedFirstTranche(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
