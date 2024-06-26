using System.Globalization;
using System.Text.RegularExpressions;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.BusinessLogic.Projects.Consts;

namespace HE.Investments.Loans.BusinessLogic.Projects.ValueObjects;
public partial class HomesCount : ValueObject
{
    public HomesCount(string? value)
    {
        if (value.IsNotProvided() || !HomesCountRegex().IsMatch(value ?? string.Empty))
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

    [GeneratedRegex(@"^(?!0)[1-9]\d{0,3}$|^9999$")]
    private static partial Regex HomesCountRegex();
}
