using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class OtherPublicGrants : NumericValueObject
{
    public OtherPublicGrants(string value)
        : base(FinancialDetailsValidationFieldNames.OtherPublicBodiesGrants, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static OtherPublicGrants? From(decimal? value)
    {
        return value.HasValue ? new OtherPublicGrants(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
