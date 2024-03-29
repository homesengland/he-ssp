using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.Funding.ValueObjects;
public class Refinance : ValueObject
{
    public Refinance(string value, string? additionalInformation)
    {
        if (additionalInformation!.IsNotProvided())
        {
            OperationResult
            .New()
            .AddValidationError(nameof(FundingViewModel.RefinanceInfo), ValidationErrorMessage.EnterMoreDetails)
            .CheckErrors();
        }
        else if (additionalInformation!.Length > MaximumInputLength.LongInput)
        {
            OperationResult
            .New()
            .AddValidationError(nameof(FundingViewModel.RefinanceInfo), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.RefinanceInfo))
            .CheckErrors();
        }

        Value = value;
        AdditionalInformation = additionalInformation;
    }

    public string Value { get; }

    public string? AdditionalInformation { get; }

    public static Refinance New(string value, string? additionalInformation) => new(value, additionalInformation);

    public override string ToString()
    {
        return Value.ToString();
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
        yield return AdditionalInformation;
    }
}
