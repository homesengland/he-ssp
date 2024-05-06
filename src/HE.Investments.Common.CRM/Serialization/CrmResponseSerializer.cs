using System.Text.Json;

namespace HE.Investments.Common.CRM.Serialization;

public static class CrmResponseSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        Converters = { new BoolConverter() },
    };

    public static string Serialize<TDto>(TDto dto)
    {
        return JsonSerializer.Serialize(dto);
    }

    public static TResult? Deserialize<TResult>(string crmResponse)
    {
        if (string.IsNullOrEmpty(crmResponse))
        {
            return default;
        }

        return JsonSerializer.Deserialize<TResult>(crmResponse, SerializerOptions)!;
    }
}
