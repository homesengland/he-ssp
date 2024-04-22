using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class RentPerWeek : TheRequiredDecimalValueObject
{
    private const string DisplayName = "per week";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public RentPerWeek(string? value, bool isCalculation, string? rentType = null)
        : base(
            value,
            nameof(RentPerWeek),
            string.Join(" ", rentType ?? "rent", DisplayName),
            MinValue,
            MaxValue,
            precision: 2,
            MessageOptions.IsMoney.WithCalculation(isCalculation))
    {
    }

    public RentPerWeek(decimal value)
        : base(value, nameof(RentPerWeek), DisplayName, MinValue, MaxValue)
    {
    }
}
