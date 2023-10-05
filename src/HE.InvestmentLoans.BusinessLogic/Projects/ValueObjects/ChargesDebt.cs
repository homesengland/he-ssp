using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class ChargesDebt : ValueObject
{
    public ChargesDebt(bool exist, string? value)
    {
        if (exist && value.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(nameof(ChargesDebt), ValidationErrorMessage.EnterExistingLegal)
                .CheckErrors();
        }

        if (value.IsProvided() && value?.Length >= MaximumInputLength.ShortInput)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(ChargesDebt), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.ChargesDebtInfo))
                .CheckErrors();
        }

        Exist = exist;
        Value = value ?? string.Empty;
    }

    public bool Exist { get; }

    public string Value { get; }

    public static ChargesDebt From(string existsString, string? value)
    {
        var exists = existsString.MapToNonNullableBool();

        if (!exists)
        {
            return new ChargesDebt(exists, null);
        }

        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(nameof(ChargesDebt), ValidationErrorMessage.EnterExistingLegal)
                .CheckErrors();
        }

        return new ChargesDebt(exists, value);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Exist;
        yield return Value;
    }
}
