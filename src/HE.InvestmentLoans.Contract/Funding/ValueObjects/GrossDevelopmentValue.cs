using System.Globalization;
using System.Text.RegularExpressions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class GrossDevelopmentValue : ValueObject
{
    public GrossDevelopmentValue(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static GrossDevelopmentValue New(decimal value) => new(value);

    public static GrossDevelopmentValue FromString(string grossDevelopmentValue)
    {
        if (!Regex.IsMatch(grossDevelopmentValue, @"^[0-9]+([.][0-9]{1,2})?$"))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.GrossDevelopmentValue), ValidationErrorMessage.EstimatedPoundInput("GDV"))
                .CheckErrors();
        }

        _ = decimal.TryParse(grossDevelopmentValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue);
        return new GrossDevelopmentValue(parsedValue);
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
