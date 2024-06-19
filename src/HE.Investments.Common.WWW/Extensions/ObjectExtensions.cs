using System.Reflection;

namespace HE.Investments.Common.WWW.Extensions;

public static class ObjectExtensions
{
    public static TValue? GetPropertyValue<TValue>(this object item, string propertyName)
    {
        var itemType = item.GetType();
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
        var dynamicRouteData = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
        if (routeValues != null)
        {
            foreach (var property in routeValues.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                dynamicRouteData.Add(property.Name, property.GetValue(routeValues)!);
            }
        }

        if (additionalRouteValues != null)
        {
            foreach (var property in additionalRouteValues.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!dynamicRouteData.ContainsKey(property.Name))
                {
                    dynamicRouteData.Add(property.Name, property.GetValue(additionalRouteValues)!);
                }
            }
        }

        return dynamicRouteData;
    }
}
