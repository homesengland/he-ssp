using HE.InvestmentLoans.BusinessLogic.Projects.Consts;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class LandRegistryTitleNumber : ValueObject
{
    public LandRegistryTitleNumber(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(ProjectValidationFieldNames.LandRegistryTitleNumber, ValidationErrorMessage.EnterLandRegistryTitleNumber)
                .CheckErrors();
        }

        if (value!.Length > MaximumInputLength.LongInput)
        {
            OperationResult.New()
                .AddValidationError(ProjectValidationFieldNames.LandRegistryTitleNumber, ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.LocationLandRegistry))
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
