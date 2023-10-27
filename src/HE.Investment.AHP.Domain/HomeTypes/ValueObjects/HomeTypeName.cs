using System.Text.RegularExpressions;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Common;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeTypeName : ShortText
{
    private static readonly Regex NumberSuffixRegex = new Regex(@" - \d", RegexOptions.Compiled);

    public HomeTypeName(string? value) : base(value, nameof(HomeTypeName))
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
