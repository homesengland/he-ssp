using Microsoft.AspNetCore.WebUtilities;

namespace HE.Investment.AHP.WWW.Extensions;

public static class HttpRequestExtensions
{
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
}
