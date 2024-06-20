using System.Dynamic;
using System.Reflection;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Common.WWW.Extensions;

public static class ObjectExtensions
{
    public static TValue? GetPropertyValue<TValue>(this object item, string propertyName)
    {
        var itemType = item.GetType();

        if (itemType == typeof(ExpandoObject))
        {
            return ((ExpandoObject)item).GetValue<TValue>(propertyName);
        }

        var itemProperty = itemType.GetProperty(propertyName);
        if (itemProperty == null)
        {
            return default;
        }

        var itemValue = itemProperty.GetValue(item, null);
        return itemValue != null ? (TValue)itemValue : default;
    }

    public static object ExpandRouteValues(this object? routeValues, object? additionalRouteValues)
    {
        var dynamicRouteData = new ExpandoObject() as IDictionary<string, object>;

        AddPropertiesToDictionary(routeValues, dynamicRouteData);
        AddPropertiesToDictionary(additionalRouteValues, dynamicRouteData);

        return dynamicRouteData;
    }

    private static void AddPropertiesToDictionary(object? values, IDictionary<string, object> dictionary)
    {
        if (values is ExpandoObject expando)
        {
            foreach (var kvp in expando)
            {
                if (kvp.Value.IsProvided() && !dictionary.ContainsKey(kvp.Key))
                {
                    dictionary.Add(kvp.Key, kvp.Value!);
                }
            }
        }
        else if (values != null)
        {
            foreach (var property in values.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = property.GetValue(values);
                if (value != null && !dictionary.ContainsKey(property.Name))
                {
                    dictionary.Add(property.Name, value);
                }
            }
        }
    }
}
