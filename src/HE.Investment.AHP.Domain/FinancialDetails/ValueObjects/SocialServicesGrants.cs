using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class SocialServicesGrants : ValueObject
{
    public SocialServicesGrants(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.RentalIncomeBorrowing, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.GenericAmountValidationError)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.GenericAmountValidationError);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    public static SocialServicesGrants? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        return new SocialServicesGrants(value.ToWholeNumberString() ?? string.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
