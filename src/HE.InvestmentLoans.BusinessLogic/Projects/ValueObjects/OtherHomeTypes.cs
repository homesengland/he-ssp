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
public class OtherHomeType : ValueObject
{
    public OtherHomeType(string value)
    {
        if (value.IsNotProvided())
        {
            return;
        }

        if (value.Length > MaximumInputLength.ShortInput)
        {
            // TODO
            OperationResult
                .New()
                .AddValidationError(nameof(OtherHomeType), ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.OtherHomeType))
                .CheckErrors();
        }

        Value = value;
    }

    // public static HomesTypes Default;
    public string Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
