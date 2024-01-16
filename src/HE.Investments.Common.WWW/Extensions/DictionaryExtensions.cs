using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Common.WWW.Extensions;

public static class DictionaryExtensions
{
    public static IList<ExtendedSelectListItem> ToExtendedSelectList<TEnum>(this Dictionary<TEnum, string> values)
        where TEnum : Enum
    {
        return values.Select(r => new ExtendedSelectListItem(
                r.Key.GetDescription(),
                r.Key.ToString(),
                false,
                r.Value))
            .ToList();
    }
}
