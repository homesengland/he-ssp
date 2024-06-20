using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.Investments.Api.Serialization;

// TODO: remove this class
public static class ApiSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new BoolConverter() },
    };

    public static string Serialize<TDto>(TDto dto)
    {
        return JsonSerializer.Serialize(dto, Options);
    }
}
