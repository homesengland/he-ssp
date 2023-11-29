using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedWorksCosts : NumericValueObject
{
    public ExpectedWorksCosts(string value)
        : base(FinancialDetailsValidationFieldNames.ExpectedWorksCosts, value, FinancialDetailsValidationErrors.InvalidExpectedWorksCosts)
    {
    }

    public static ExpectedWorksCosts? From(decimal? value)
    {
        return value.HasValue ? new ExpectedWorksCosts(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
