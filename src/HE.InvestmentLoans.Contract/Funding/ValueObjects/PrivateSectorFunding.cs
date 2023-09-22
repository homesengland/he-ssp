using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class PrivateSectorFunding : ValueObject
{
    public PrivateSectorFunding(bool isApplied, string? privateSectorFundingApplyResult, string? privateSectorFundingNotApplyingReason)
    {
        if (isApplied)
        {
            if (privateSectorFundingApplyResult.IsNotProvided())
            {
                OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.PrivateSectorFundingResult), ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
            }
            else if (privateSectorFundingApplyResult!.Length >= MaximumInputLength.LongInput)
            {
                OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.PrivateSectorFundingResult), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.PrivateSectorFundingResult))
                .CheckErrors();
            }
        }
        else
        {
            if (privateSectorFundingNotApplyingReason.IsNotProvided())
            {
                OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.PrivateSectorFundingReason), ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
            }
            else if (privateSectorFundingNotApplyingReason!.Length >= MaximumInputLength.LongInput)
            {
                OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.PrivateSectorFundingReason), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.PrivateSectorFundingReason))
                .CheckErrors();
            }
        }

        IsApplied = isApplied;

        if (isApplied)
        {
            PrivateSectorFundingApplyResult = privateSectorFundingApplyResult;
            PrivateSectorFundingNotApplyingReason = null;
        }
        else
        {
            PrivateSectorFundingApplyResult = null;
            PrivateSectorFundingNotApplyingReason = privateSectorFundingNotApplyingReason;
        }
    }

    public bool IsApplied { get; }

    public string? PrivateSectorFundingApplyResult { get; }

    public string? PrivateSectorFundingNotApplyingReason { get; }

    public static PrivateSectorFunding New(bool isApplied, string? privateSectorFundingApplyResult, string? privateSectorFundingNotApplyingReason) => new(isApplied, privateSectorFundingApplyResult, privateSectorFundingNotApplyingReason);

    public static PrivateSectorFunding FromString(string isApplied, string? privateSectorFundingApplyResult, string? privateSectorFundingNotApplyingReason)
    {
        return new PrivateSectorFunding(isApplied == CommonResponse.Yes, privateSectorFundingApplyResult, privateSectorFundingNotApplyingReason);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsApplied;
        yield return PrivateSectorFundingApplyResult;
        yield return PrivateSectorFundingNotApplyingReason;
    }
}
