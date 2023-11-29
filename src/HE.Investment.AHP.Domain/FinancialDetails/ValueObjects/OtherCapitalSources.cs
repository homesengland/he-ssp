using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class OtherCapitalSources : NumericValueObject
{
    public OtherCapitalSources(string value)
        : base(FinancialDetailsValidationFieldNames.OtherCapitalSources, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static OtherCapitalSources? From(decimal? value)
    {
        return value.HasValue ? new OtherCapitalSources(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
