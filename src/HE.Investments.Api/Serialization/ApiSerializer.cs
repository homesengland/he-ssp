using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.Investments.Api.Serialization;

public static class ApiSerializer
{
    public static JsonSerializerOptions Options => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new BoolConverter() },
    };

    public static string Serialize<TDto>(TDto dto)
    {
        return JsonSerializer.Serialize(dto, Options);
    }
}
