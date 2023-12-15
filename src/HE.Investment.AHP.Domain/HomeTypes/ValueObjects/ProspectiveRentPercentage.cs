using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class ProspectiveRentPercentage : ValueObject
{
    public ProspectiveRentPercentage(decimal? value, string fieldName)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.CouldNotCalculate)
                .CheckErrors();
        }

        Value = value!.Value;
    }

    public ProspectiveRentPercentage(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString("0", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
