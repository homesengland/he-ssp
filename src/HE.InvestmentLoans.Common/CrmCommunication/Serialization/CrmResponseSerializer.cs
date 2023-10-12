using System.Text.Json;
using System.Text.Json.Serialization;

namespace HE.InvestmentLoans.Common.CrmCommunication.Serialization;

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
