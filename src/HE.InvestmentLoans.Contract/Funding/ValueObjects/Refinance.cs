using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class Refinance : ValueObject
{
    public Refinance(string value, string? additionalInformation)
    {
        if (additionalInformation.IsNotProvided())
        {
            OperationResult
            .New()
            .AddValidationError(nameof(FundingViewModel.RefinanceInfo), ValidationErrorMessage.EnterMoreDetails)
            .CheckErrors();
        }
        else if (additionalInformation!.Length >= MaximumInputLength.LongInput)
        {
            OperationResult
            .New()
            .AddValidationError(nameof(FundingViewModel.RefinanceInfo), ValidationErrorMessage.InputLongerThanThousandCharacters)
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
