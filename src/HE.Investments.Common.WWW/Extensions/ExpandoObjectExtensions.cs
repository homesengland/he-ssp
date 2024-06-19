using System.Dynamic;

namespace HE.Investments.Common.WWW.Extensions;

public static class ExpandoObjectExtensions
{
    public static TValue? GetValue<TValue>(this ExpandoObject expandoObject, string key)
    {
        if (expandoObject is not IDictionary<string, object> dictionary || !dictionary.TryGetValue(key, out var value))
        {
            return default;
        }

        return (TValue)value;
    }
}
