using System.Globalization;
using System.Text.RegularExpressions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Domain;

namespace HE.InvestmentLoans.Contract.Funding.ValueObjects;
public class GrossDevelopmentValue : ValueObject
{
    public GrossDevelopmentValue(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static GrossDevelopmentValue FromString(string grossDevelopmentValue)
    {
        if (!Regex.IsMatch(grossDevelopmentValue, @"^[0-9]+([.,][0-9]{1,2})?$"))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(FundingViewModel.GrossDevelopmentValue), ValidationErrorMessage.EstimatedPoundInput("GDV"))
                .CheckErrors();
        }

        _ = int.TryParse(grossDevelopmentValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue);
        return new GrossDevelopmentValue(parsedValue);
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
