using AngleSharp.Html.Dom;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O02Allocation.Extensions;

public static class HtmlAnchorElementExtensions
{
    public static string ExtractParameter(this IHtmlAnchorElement link, string parameterName, string template)
    {
        var parameterIndex = Array.IndexOf(template.Split("/"), $"{{{parameterName}}}");
        if (parameterIndex < 0)
        {
            throw new InvalidOperationException("Template does not match link URL.");
        }

        return link.Href.Replace("https://localhost/", string.Empty).Split("/")[parameterIndex];
    }
}
