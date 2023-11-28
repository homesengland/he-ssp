using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class CountyCouncilGrants : NumericValueObject
{
    public CountyCouncilGrants(string value)
        : base(FinancialDetailsValidationFieldNames.CountyCouncilGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static CountyCouncilGrants? From(decimal? value)
    {
        return value.HasValue ? new CountyCouncilGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
