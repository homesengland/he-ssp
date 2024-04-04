using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.CRM.Mappers;

public static class DomainEnumMapper
{
    public static TSource? Map<TSource>(int? value, IDictionary<TSource, int?> mapping, TSource? defaultValue = default)
        where TSource : struct
    {
        if (value == null)
        {
            return defaultValue;
        }

        if (!mapping.TryGetKeyByValue(value.Value, out var result))
        {
            throw new ArgumentException($"Cannot find {typeof(TSource).Name} Domain mapping for {value}");
        }

        return result;
    }

    public static IList<TSource> Map<TSource>(IEnumerable<int> values, IDictionary<TSource, int?> mapping)
        where TSource : struct
    {
        return values.Select(x => Map(x, mapping)!.Value).ToList();
    }
}
