using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Security.ValueObjects;
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
                .AddValidationError("DirLoansSub", ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
        }

        if (reasonWhyCannotBeSubordinated.Length > MaximumInputLength.LongInput)
        {
            OperationResult
                .New()
                .AddValidationError(nameof(CanBeSubordinated), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.SubordinatedLoans))
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
