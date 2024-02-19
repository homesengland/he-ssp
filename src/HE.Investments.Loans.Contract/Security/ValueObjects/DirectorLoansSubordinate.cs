using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.Security.ValueObjects;
public class DirectorLoansSubordinate : ValueObject
{
    public DirectorLoansSubordinate(bool canBeSubordinated, string reasonWhyCannotBeSubordinated)
    {
        CanBeSubordinated = canBeSubordinated;

        if (CanBeSubordinated)
        {
            ReasonWhyCannotBeSubordinated = string.Empty;
            return;
        }

        if (reasonWhyCannotBeSubordinated.IsNotProvided())
        {
            OperationResult
                .New()
                .AddValidationError("DirLoansSubMore", ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
        }

        if (reasonWhyCannotBeSubordinated.Length > MaximumInputLength.LongInput)
        {
            OperationResult
                .New()
                .AddValidationError("DirLoansSubMore", ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.SubordinatedLoans))
                .CheckErrors();
        }

        ReasonWhyCannotBeSubordinated = reasonWhyCannotBeSubordinated;
    }

    public bool CanBeSubordinated { get; }

    public string ReasonWhyCannotBeSubordinated { get; }

    public static DirectorLoansSubordinate FromString(string canBeSubordinated, string reasonWhyCannotBeSubordinated) =>
        new(canBeSubordinated.MapToNonNullableBool(), reasonWhyCannotBeSubordinated);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CanBeSubordinated;
        yield return ReasonWhyCannotBeSubordinated;
    }
}
