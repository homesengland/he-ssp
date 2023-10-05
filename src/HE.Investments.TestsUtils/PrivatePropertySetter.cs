using System.Reflection;

namespace HE.Investments.TestsUtils;

public static class PrivatePropertySetter
{
    public static void SetPropertyWithNoSetter<T>(T objectWithPropertyToSet, string propertyName, object? valueToSet)
    {
        var prop = GetField(typeof(T), $"<{propertyName}>k__BackingField");
        prop?.SetValue(objectWithPropertyToSet, valueToSet);
    }

    public static void SetPrivateField<T>(T objectWithPropertyToSet, string fieldName, object valueToSet)
    {
        var prop = GetField(typeof(T), fieldName);
        prop?.SetValue(objectWithPropertyToSet, valueToSet);
    }

    private static FieldInfo? GetField(Type? type, string name)
    {
        if (type?.BaseType == null)
        {
            return null;
        }

        var baseField = GetField(type.BaseType, name);
        if (baseField != null)
        {
            return baseField;
        }

        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
    }
}
