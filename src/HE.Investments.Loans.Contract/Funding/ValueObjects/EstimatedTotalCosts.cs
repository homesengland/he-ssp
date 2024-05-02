using System.Globalization;
using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.Contract.Funding.ValueObjects;
public partial class EstimatedTotalCosts : MoneyValueObject
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
        if (!EstimatedTotalCostsRegex().IsMatch(estimatedTotalCosts.Trim()))
        {
            OperationResult.ThrowValidationError(nameof(FundingViewModel.TotalCosts), ValidationErrorMessage.EstimatedPoundInput("total cost"));
        }

        return new EstimatedTotalCosts(estimatedTotalCosts);
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    [GeneratedRegex("^[0-9]+([.][0-9]{1,2})?$")]
    private static partial Regex EstimatedTotalCostsRegex();
}
