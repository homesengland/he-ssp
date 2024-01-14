using System.Text.RegularExpressions;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeTypeName : ShortText
{
    private static readonly Regex NumberSuffixRegex = new(@" - \d", RegexOptions.Compiled);

    public HomeTypeName(string? value)
        : base(value, nameof(HomeTypeName), "home type name")
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

    public override string ToString()
    {
        return Value;
    }

    private static string Suffix(int suffixIndex)
    {
        return $" - {suffixIndex}";
    }
}
