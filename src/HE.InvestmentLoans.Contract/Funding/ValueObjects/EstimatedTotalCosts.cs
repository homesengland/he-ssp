using System.Globalization;
using System.Text.RegularExpressions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class EstimatedTotalCosts : ValueObject
{
    public EstimatedTotalCosts(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static EstimatedTotalCosts FromString(string estimatedTotalCosts)
    {
        if (!Regex.IsMatch(estimatedTotalCosts, @"^[0-9]+([.,][0-9]{1,2})?$"))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.TotalCosts), ValidationErrorMessage.EstimatedPoundInput("total cost"))
                .CheckErrors();
        }

        _ = int.TryParse(estimatedTotalCosts, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
        return new EstimatedTotalCosts(parsedValue);
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
