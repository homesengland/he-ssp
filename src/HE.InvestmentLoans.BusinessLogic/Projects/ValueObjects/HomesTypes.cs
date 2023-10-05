using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class HomesTypes : ValueObject
{
    public HomesTypes(string[] homesTypes, string otherHomesTypes)
    {
        if (homesTypes.IsProvided() && homesTypes.Contains(CommonResponse.Other) && otherHomesTypes.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesTypes), ValidationErrorMessage.TypeHomesOtherType)
                .CheckErrors();
        }

        if (otherHomesTypes.IsProvided() && otherHomesTypes.Length >= MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(HomesTypes), ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.OtherHomeType))
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
