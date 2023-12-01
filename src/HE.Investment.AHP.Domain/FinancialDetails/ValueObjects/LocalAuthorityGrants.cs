using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LocalAuthorityGrants : NumericValueObject
{
    public LocalAuthorityGrants(string value)
        : base(FinancialDetailsValidationFieldNames.LocalAuthorityGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static LocalAuthorityGrants? From(decimal? value)
    {
        return value.HasValue ? new LocalAuthorityGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
