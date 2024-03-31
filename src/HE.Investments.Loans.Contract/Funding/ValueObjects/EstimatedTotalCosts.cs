using System.Globalization;
using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Funding.ValueObjects;
public class EstimatedTotalCosts : MoneyValueObject
{
    private const string DisplayName = "Estimated Total Costs";

    public EstimatedTotalCosts(string value)
        : base(value, nameof(EstimatedTotalCosts), DisplayName)
    {
    }

    public EstimatedTotalCosts(decimal value)
        : base(value, nameof(EstimatedTotalCosts), DisplayName)
    {
    }

    public static EstimatedTotalCosts New(decimal value) => new(value);

    public static EstimatedTotalCosts FromString(string estimatedTotalCosts)
    {
        if (!Regex.IsMatch(estimatedTotalCosts.Trim(), "^[0-9]+([.][0-9]{1,2})?$"))
        {
            OperationResult.ThrowValidationError(nameof(FundingViewModel.TotalCosts), ValidationErrorMessage.EstimatedPoundInput("total cost"));
        }

        return new EstimatedTotalCosts(estimatedTotalCosts);
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
