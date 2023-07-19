using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

<<<<<<<< HEAD:src/HE.InvestmentLoans.BusinessLogic/LoanApplicationLegacy/Extensions/SessionExtensions.cs
namespace HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Extensions;
========
namespace HE.InvestmentLoans.Common.Extensions;
>>>>>>>> 3bc1fb6 (Error handler redirect):src/HE.InvestmentLoans.Common/Extensions/SessionExtensions.cs

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
