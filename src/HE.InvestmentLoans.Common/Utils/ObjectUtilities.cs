using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.InvestmentLoans.Common.Utils;
public static class ObjectUtilities
{
    public static T DeepCopy<T>(T obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj), "Object cannot be null.");
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
        };

        var serializedObject = JsonSerializer.Serialize(obj, options);

        return JsonSerializer.Deserialize<T>(serializedObject, options)!;
    }
}
