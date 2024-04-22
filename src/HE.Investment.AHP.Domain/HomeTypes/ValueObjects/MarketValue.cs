using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MarketValue : TheRequiredIntValueObject
{
    private const string DisplayName = "market value of each home";

    private const int MinValue = 0;

    private const int MaxValue = 99999999;

    public MarketValue(string? value, bool isCalculation)
        : base(value, nameof(MarketValue), DisplayName, MinValue, MaxValue, MessageOptions.IsMoney.WithCalculation(isCalculation))
    {
    }

    public MarketValue(int value)
        : base(value, nameof(MarketValue), DisplayName, MinValue, MaxValue)
    {
    }
}
