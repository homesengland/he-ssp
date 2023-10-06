using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class LandRegistryTitleNumber : ValueObject
{
    public LandRegistryTitleNumber(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError("LocationLandRegistry", ValidationErrorMessage.EnterLandRegistryTitleNumber)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError("LocationLandRegistry", ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationLandRegistry))
                .CheckErrors();
        }

        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
