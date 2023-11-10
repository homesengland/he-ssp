using System.Globalization;
using System.Text.RegularExpressions;
using HE.InvestmentLoans.BusinessLogic.Projects.Consts;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
public class HomesCount : ValueObject
{
    public HomesCount(string? value)
    {
        if (value.IsNotProvided() || !Regex.IsMatch(value ?? string.Empty, @"^(?!0)[1-9]\d{0,3}$|^9999$"))
        {
            OperationResult
                .New()
                .AddValidationError(ProjectValidationFieldNames.HomesCount, ValidationErrorMessage.ManyHomesAmount)
                .CheckErrors();
        }

        Value = value ?? string.Empty;
    }

    public static HomesCount Default => new("0");

    public string Value { get; }

    public int AsInt() => int.Parse(Value, CultureInfo.InvariantCulture);

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
