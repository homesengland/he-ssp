using HE.InvestmentLoans.BusinessLogic.Projects.Consts;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class HomesTypes : ValueObject
{
    public HomesTypes(string[] homesTypes, string otherHomesTypes)
    {
        if (homesTypes.IsProvided() && homesTypes.Contains(CommonResponse.Other) && otherHomesTypes.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.OtherHomeType, ValidationErrorMessage.TypeHomesOtherType)
                .CheckErrors();
        }

        if (otherHomesTypes.IsProvided() && otherHomesTypes.Length >= MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.OtherHomeType, ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.OtherHomeType))
                .CheckErrors();
        }

        HomesTypesValue = homesTypes;
        OtherHomesTypesValue = otherHomesTypes;
    }

    public string[] HomesTypesValue { get; }

    public string OtherHomesTypesValue { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return HomesTypesValue;
        yield return OtherHomesTypesValue;
    }
}
