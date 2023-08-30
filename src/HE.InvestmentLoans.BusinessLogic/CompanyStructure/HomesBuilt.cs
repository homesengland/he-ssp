using System.Globalization;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure;

public class HomesBuilt : ValueObject
{
    public HomesBuilt(int value)
    {
        if (value is < 0 or > 99999)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesBuilt), ValidationErrorMessage.HomesBuiltIncorrectNumber)
                .ThrowException();
        }

        Value = value;
    }

    public int Value { get; }

    public static HomesBuilt FromString(string homesBuildAsString)
    {
        if (decimal.TryParse(homesBuildAsString, NumberStyles.Float, CultureInfo.InvariantCulture, out var decimalResult))
        {
            if (decimalResult % 1 != 0)
            {
                OperationResult
                    .New()
                    .AddValidationError(nameof(HomesBuilt), ValidationErrorMessage.HomesBuiltDecimalNumber)
                    .ThrowException();
            }
        }

        if (!int.TryParse(homesBuildAsString, out var intValue))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesBuilt), ValidationErrorMessage.HomesBuiltIncorretInput)
                .ThrowException();
        }

        return new HomesBuilt(intValue);
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
