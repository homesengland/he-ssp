using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class HomesTransferValue : NumericValueObject
{
    public HomesTransferValue(string value)
        : base(FinancialDetailsValidationFieldNames.HomesTransferValue, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static HomesTransferValue? From(decimal? value)
    {
        return value.HasValue ? new HomesTransferValue(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
