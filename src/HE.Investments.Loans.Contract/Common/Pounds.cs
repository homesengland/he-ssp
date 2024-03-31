using System.Globalization;
using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Loans.Contract.Common;
public class Pounds : MoneyValueObject
{
    public Pounds(string value, string fieldName, string displayName)
        : base(value, fieldName, displayName)
    {
    }

    public Pounds(decimal value, string fieldName = nameof(Pounds), string displayName = "pounds value")
        : base(value, fieldName, displayName)
    {
    }

    public static Pounds New(decimal value, string fieldName, string displayName) => new(value, fieldName, displayName);

    public static Pounds FromString(string value, string fieldName, string displayName)
    {
        if (value.IsNotProvided() || !Regex.IsMatch(value, @"^[0-9]+([.][0-9]{1,2})?$"))
        {
            OperationResult
                .New()
                .AddValidationError(fieldName, GenericValidationError.InvalidPoundsValue)
                .CheckErrors();
        }

        return new Pounds(value, fieldName, displayName);
    }

    public override string ToString()
    {
        return Value.ToString("0.##", CultureInfo.InvariantCulture);
    }
}
