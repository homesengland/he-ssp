using HE.Investments.Common.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

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

    public static bool TryGetWorkflowQueryParameter(this HttpRequest httpRequest, out string workflow)
    {
        if (QueryHelpers.ParseQuery(httpRequest.QueryString.Value).TryGetValue("workflow", out var lastEncodedWorkflow))
        {
            workflow = lastEncodedWorkflow.ToString();
            return true;
        }

        workflow = string.Empty;
        return false;
    }

    public static bool TryGetCallbackQueryParameter(this HttpRequest httpRequest, out string callbackUrl)
    {
        if (QueryHelpers.ParseQuery(httpRequest.QueryString.Value).TryGetValue("callback", out var callback))
        {
            callbackUrl = callback.ToString();
            return true;
        }

        callbackUrl = string.Empty;
        return false;
    }
}
