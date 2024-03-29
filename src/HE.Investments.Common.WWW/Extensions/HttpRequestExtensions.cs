using HE.Investments.Common.Messages;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.Common.WWW.Extensions;

public static class HttpRequestExtensions
{
    public static string? GetRouteValue(this HttpRequest request, string name)
    {
        return request.RouteValues.FirstOrDefault(x => x.Key == name).Value as string;
    }

    public static bool IsSaveAndReturnAction(this HttpRequest request)
    {
        return request.Form["action"] == GenericMessages.SaveAndReturn;
    }
}
