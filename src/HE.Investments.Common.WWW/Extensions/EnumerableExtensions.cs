using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Models;

namespace HE.Investments.Common.WWW.Extensions;

public static class EnumerableExtensions
{
    public static IList<ExtendedSelectListItem> ToSelectList<TEnum>(this IEnumerable<TEnum> values)
        where TEnum : Enum
    {
        return values.Select(r => new ExtendedSelectListItem(
                r.GetDescription(),
                r.ToString(),
                false))
            .ToList();
    }
}
