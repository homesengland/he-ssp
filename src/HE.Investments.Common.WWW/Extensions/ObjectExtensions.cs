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
}
