using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class TargetRentExceedMarketRent : ValueObject
{
    private const string DisplayName = "target rent plus service charge for these home exceed 80%";

    public TargetRentExceedMarketRent(YesNoType value, bool isCalculation = false)
    {
        if (value.IsNotProvided() && !isCalculation)
        {
            Value = YesNoType.Undefined;
            return;
        }

        if (value.IsNotProvided() && isCalculation)
        {
            OperationResult.New()
                .AddValidationError(nameof(TargetRentExceedMarketRent), ValidationErrorMessage.MustBeSelectedForCalculation(DisplayName))
                .CheckErrors();
        }

        Value = value;
    }

    public YesNoType Value { get; }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
