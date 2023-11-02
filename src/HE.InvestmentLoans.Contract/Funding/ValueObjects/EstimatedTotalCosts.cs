using System.Globalization;
using System.Text.RegularExpressions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class EstimatedTotalCosts : ValueObject
{
    public EstimatedTotalCosts(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static EstimatedTotalCosts New(decimal value) => new(value);

    public static EstimatedTotalCosts FromString(string estimatedTotalCosts)
    {
        if (!Regex.IsMatch(estimatedTotalCosts, @"^[0-9]+([.][0-9]{1,2})?$"))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.TotalCosts), ValidationErrorMessage.EstimatedPoundInput("total cost"))
                .CheckErrors();
        }

        _ = decimal.TryParse(estimatedTotalCosts, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue);
        return new EstimatedTotalCosts(parsedValue);
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
