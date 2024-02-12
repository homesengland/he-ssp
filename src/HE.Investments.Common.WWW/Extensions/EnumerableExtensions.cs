using System.Reflection;
using HE.Investments.Common.Contract.Enum;
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
                false,
                r.GetAttribute<HintAttribute>()?.Text))
            .ToList();
    }

    public static T? GetAttribute<T>(this Enum enumValue)
        where T : Attribute
    {
        T attribute;

        var memberInfo = enumValue.GetType().GetMember(enumValue.ToString())
                                        .FirstOrDefault();

        if (memberInfo != null)
        {
            var attr = memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            if (attr != null)
            {
                attribute = (T)attr;
                return attribute;
            }
        }

        return null;
    }
}
