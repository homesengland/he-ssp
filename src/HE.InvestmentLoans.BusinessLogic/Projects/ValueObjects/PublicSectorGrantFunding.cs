using HE.InvestmentLoans.BusinessLogic.Projects.Consts;
using HE.InvestmentLoans.Common.Domain;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Common;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;

public class PublicSectorGrantFunding : ValueObject
{
    public PublicSectorGrantFunding(ShortText? providerName, Pounds? amount, ShortText? grantOrFundName, LongText? purpose)
    {
        ProviderName = providerName;
        Amount = amount;
        GrantOrFundName = grantOrFundName;
        Purpose = purpose;
    }

    public ShortText? ProviderName { get; private set; }

    public Pounds? Amount { get; private set; }

    public ShortText? GrantOrFundName { get; private set; }

    public LongText? Purpose { get; private set; }

    public static PublicSectorGrantFunding FromString(string providerNameString, string amountString, string grantOrFoundNameString, string purposeString)
    {
        var aggregatedResult = OperationResult.New();

        var providerName = providerNameString.IsProvided() ? aggregatedResult.CatchResult(() => new ShortText(providerNameString)) : null;
        aggregatedResult.OverrideError(GenericValidationError.TextTooLong, ProjectValidationFieldNames.GrantFunding.ProviderName, ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingProviderName));

        var amount = amountString.IsProvided() ? aggregatedResult.CatchResult(() => Pounds.FromString(amountString)) : null;
        aggregatedResult.OverrideError(GenericValidationError.InvalidPoundsValue, ProjectValidationFieldNames.GrantFunding.Amount, ValidationErrorMessage.IncorrectGrantFundingAmount);

        var grantOrFoundName = grantOrFoundNameString.IsProvided() ? aggregatedResult.CatchResult(() => new ShortText(grantOrFoundNameString)) : null;
        aggregatedResult.OverrideError(GenericValidationError.TextTooLong, ProjectValidationFieldNames.GrantFunding.FundingName, ValidationErrorMessage.ShortInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingName));

        var purpose = purposeString.IsProvided() ? aggregatedResult.CatchResult(() => new LongText(purposeString)) : null;
        aggregatedResult.OverrideError(GenericValidationError.TextTooLong, ProjectValidationFieldNames.GrantFunding.Purpose, ValidationErrorMessage.LongInputLengthExceeded(FieldNameForInputLengthValidation.GrantFundingPurpose));

        aggregatedResult.CheckErrors();

        return new PublicSectorGrantFunding(providerName, amount, grantOrFoundName, purpose);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return ProviderName!;
        yield return Amount!;
        yield return GrantOrFundName!;
        yield return Purpose!;
    }
}
