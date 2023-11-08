using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
public class LandOwnership : ValueObject
{
    public LandOwnership(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.LandOwnership, FinancialDetailsValidationErrors.NoLandOwnershipProvided)
                .CheckErrors();
        }

        try
        {
            var mappedValue = value.MapToNonNullableBool();
            Value = mappedValue;
        }
        catch (ArgumentException)
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.LandOwnership, FinancialDetailsValidationErrors.NoLandOwnershipProvided)
                .CheckErrors();
        }
    }

    public bool Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
