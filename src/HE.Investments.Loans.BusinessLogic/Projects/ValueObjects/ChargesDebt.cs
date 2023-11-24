using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;
using HE.Investments.Loans.Common.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public class ChargesDebt : ValueObject
{
    public ChargesDebt(bool exist, string? info)
    {
        if (exist && info.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.ChargesDebtInfo, ValidationErrorMessage.EnterExistingLegal)
                .CheckErrors();
        }

        if (info.IsProvided() && info?.Length >= MaximumInputLength.LongInput)
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.ChargesDebtInfo, ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.ChargesDebtInfo))
                .CheckErrors();
        }

        Exist = exist;
        Info = info ?? string.Empty;
    }

    public bool Exist { get; }

    public string Info { get; }

    public static ChargesDebt From(string existsString, string? info)
    {
        var exists = existsString.MapToNonNullableBool();

        if (!exists)
        {
            return new ChargesDebt(exists, null);
        }

        if (info.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError("ChargesDebtInfo", ValidationErrorMessage.EnterExistingLegal)
                .CheckErrors();
        }

        return new ChargesDebt(exists, info);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Exist;
        yield return Info;
    }
}
