using System.Globalization;
using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.Common;

public class Pounds : ValueObject
{
    public Pounds(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static Pounds FromString(string? estimatedTotalCosts)
    {
        if (estimatedTotalCosts.IsNotProvided() || !Regex.IsMatch(estimatedTotalCosts!, @"^[0-9]+([.][0-9]{1,2})?$"))
        {
            OperationResult
                .New()
                .AddValidationError(nameof(Pounds), GenericValidationError.InvalidPoundsValue)
                .CheckErrors();
        }

        _ = decimal.TryParse(estimatedTotalCosts, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue);

        return new Pounds(parsedValue);
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
