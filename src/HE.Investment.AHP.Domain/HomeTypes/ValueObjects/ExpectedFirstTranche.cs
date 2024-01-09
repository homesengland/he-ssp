using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class ExpectedFirstTranche : ValueObject
{
    public ExpectedFirstTranche(decimal? value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(ExpectedFirstTranche), ValidationErrorMessage.CouldNotCalculate)
                .CheckErrors();
        }

        Value = value!.Value;
    }

    public ExpectedFirstTranche(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString("0.00", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
