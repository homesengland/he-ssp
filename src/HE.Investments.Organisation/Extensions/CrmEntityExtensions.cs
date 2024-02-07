using Microsoft.Xrm.Sdk;

namespace HE.Investments.Organisation.Extensions;

public static class CrmEntityExtensions
{
    public static string? GetStringAttribute(this Entity entity, string attributeName)
    {
        return entity.Contains(attributeName) ? entity[attributeName].ToString() : null;
    }

    public static bool? GetBooleanAttribute(this Entity entity, string attributeName)
    {
        if (entity.Contains(attributeName) && bool.TryParse(entity[attributeName].ToString(), out var attributeValue))
        {
            return attributeValue;
        }

        return null;
    }

    public static EntityReference? GetEntityReference(this Entity entity, string attributeName)
    {
        if (entity.Contains(attributeName) && entity[attributeName] != null)
        {
            return (EntityReference)entity.Attributes[attributeName];
        }

        return null;
    }
}
