using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

namespace HE.Investments.Loans.Contract.Funding.ValueObjects;
public class AbnormalCosts : ValueObject
{
    public AbnormalCosts(bool isAnyAbnormalCost, string? abnormalCostsAdditionalInformation)
    {
        if (isAnyAbnormalCost)
        {
            if (abnormalCostsAdditionalInformation!.IsNotProvided())
            {
                OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.AbnormalCostsInfo), ValidationErrorMessage.EnterMoreDetails)
                .CheckErrors();
            }
            else if (abnormalCostsAdditionalInformation!.Length > MaximumInputLength.LongInput)
            {
                OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.AbnormalCostsInfo), ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.AbnormalCostsInfo))
                .CheckErrors();
            }
        }

        IsAnyAbnormalCost = isAnyAbnormalCost;

        if (isAnyAbnormalCost)
        {
            AbnormalCostsAdditionalInformation = abnormalCostsAdditionalInformation;
        }
        else
        {
            AbnormalCostsAdditionalInformation = null;
        }
    }

    public bool IsAnyAbnormalCost { get; }

    public string? AbnormalCostsAdditionalInformation { get; }

    public static AbnormalCosts New(bool isAnyAbnormalCost, string? abnormalCostsAdditionalInformation) => new(isAnyAbnormalCost, abnormalCostsAdditionalInformation);

    public static AbnormalCosts FromString(string abnormalCostAsString, string? abnormalCostsAdditionalInformation)
    {
        return new AbnormalCosts(abnormalCostAsString == CommonResponse.Yes, abnormalCostsAdditionalInformation);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return IsAnyAbnormalCost;
        yield return AbnormalCostsAdditionalInformation;
    }
}
