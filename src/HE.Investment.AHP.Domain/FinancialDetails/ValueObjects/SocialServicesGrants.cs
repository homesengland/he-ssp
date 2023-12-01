using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class SocialServicesGrants : NumericValueObject
{
    public SocialServicesGrants(string value)
        : base(FinancialDetailsValidationFieldNames.SocialServicesGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static SocialServicesGrants? From(decimal? value)
    {
        return value.HasValue ? new SocialServicesGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
