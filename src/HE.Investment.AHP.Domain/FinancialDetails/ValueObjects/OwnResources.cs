using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class OwnResources : NumericValueObject
{
    public OwnResources(string value)
        : base(FinancialDetailsValidationFieldNames.OwnResources, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static OwnResources? From(decimal? value)
    {
        return value.HasValue ? new OwnResources(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
