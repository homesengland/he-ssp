using System.Text.Json;

namespace HE.Investments.Loans.Common.CrmCommunication.Serialization;

public static class CrmResponseSerializer
{
    public static string Serialize<TDto>(TDto dto)
    {
        return JsonSerializer.Serialize(dto);
    }

    public static TResult Deserialize<TResult>(string crmResponse)
    {
        var serializerOptions = new JsonSerializerOptions
        {
            Converters = { new BoolConverter() },
        };

        return JsonSerializer.Deserialize<TResult>(crmResponse, serializerOptions)!;
    }
}
