using System.Globalization;
using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.Contract.Funding.ValueObjects;
public class GrossDevelopmentValue : MoneyValueObject
{
    private const string DisplayName = "Gross Development Value";

    public GrossDevelopmentValue(string value)
        : base(value, nameof(GrossDevelopmentValue), DisplayName)
    {
    }

    public GrossDevelopmentValue(decimal value)
        : base(value, nameof(GrossDevelopmentValue), DisplayName)
    {
    }

    public static GrossDevelopmentValue New(decimal value) => new(value);

    public static GrossDevelopmentValue FromString(string grossDevelopmentValue)
    {
        if (!Regex.IsMatch(grossDevelopmentValue, @"^[0-9]+([.][0-9]{1,2})?$"))
        {
            OperationResult.ThrowValidationError(nameof(FundingViewModel.GrossDevelopmentValue), ValidationErrorMessage.EstimatedPoundInput("GDV"));
        }

        return new GrossDevelopmentValue(grossDevelopmentValue);
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
