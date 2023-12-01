using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedOnCosts : NumericValueObject
{
    public ExpectedOnCosts(string value)
        : base(FinancialDetailsValidationFieldNames.ExpectedOnCosts, value, FinancialDetailsValidationErrors.InvalidExpectedOnCosts)
    {
    }

    public static ExpectedOnCosts? From(decimal? value)
    {
        return value.HasValue ? new ExpectedOnCosts(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
