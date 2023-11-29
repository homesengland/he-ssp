using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class RentalIncomeBorrowing : NumericValueObject
{
    public RentalIncomeBorrowing(string value)
        : base(FinancialDetailsValidationFieldNames.RentalIncomeBorrowing, value, FinancialDetailsValidationErrors.GenericAmountValidationError)
    {
    }

    public static RentalIncomeBorrowing? From(decimal? value)
    {
        return value.HasValue ? new RentalIncomeBorrowing(value.ToWholeNumberString() ?? string.Empty) : null;
    }
}
