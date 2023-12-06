using System.Globalization;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class AffordableRentAsPercentageOfMarketRent : ValueObject
{
    public AffordableRentAsPercentageOfMarketRent(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString("00.00", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
