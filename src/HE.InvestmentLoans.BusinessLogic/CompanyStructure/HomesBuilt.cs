using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

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
                .CheckErrors();
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
                    .CheckErrors();
            }
        }

        if (!int.TryParse(homesBuildAsString, out var intValue))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesBuilt), ValidationErrorMessage.HomesBuiltIncorretInput)
                .CheckErrors();
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
