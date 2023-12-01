using System.Text.RegularExpressions;
using HE.Investments.Common.Messages;
using HE.Investments.Loans.Contract.Common;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeTypeName : ShortText
{
    private static readonly Regex NumberSuffixRegex = new(@" - \d", RegexOptions.Compiled);

    public HomeTypeName(string? value)
        : base(value, nameof(HomeTypeName), "Home type name")
    {
    }

    public HomeTypeName Duplicate(int suffixIndex)
    {
        var suffix = Suffix(suffixIndex);
        var duplicatedName = NumberSuffixRegex.Replace(Value, string.Empty);
        duplicatedName = duplicatedName[..Math.Min(MaximumInputLength.ShortInput - suffix.Length, duplicatedName.Length)];
        duplicatedName += suffix;

        return new HomeTypeName(duplicatedName);
    }

    private static string Suffix(int suffixIndex)
    {
        return $" - {suffixIndex}";
    }
}
