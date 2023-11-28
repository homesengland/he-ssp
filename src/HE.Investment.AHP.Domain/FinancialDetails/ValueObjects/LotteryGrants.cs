using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LotteryGrants : NumericValueObject
{
    public LotteryGrants(string value)
        : base(FinancialDetailsValidationFieldNames.LotteryGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static LotteryGrants? From(decimal? value)
    {
        return value.HasValue ? new LotteryGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
