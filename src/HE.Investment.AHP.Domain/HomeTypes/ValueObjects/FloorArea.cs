using System.Globalization;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class FloorArea : TheRequiredDecimalValueObject
{
    private const string DisplayName = "internal floor area of this home type";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 999.99m;

    public FloorArea(string? value)
        : base(value, nameof(FloorArea), DisplayName, MinValue, MaxValue, precision: 2, defaultErrorMessage: false)
    {
    }

    public FloorArea(decimal value)
        : base(value, nameof(FloorArea), DisplayName, MinValue, MaxValue)
    {
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
