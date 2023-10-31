using System.Globalization;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.Components;

public class LinkButtonTagHelper : TagHelper
{
    public string ActionUrl { get; set; }

    public bool IsSecondary { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (output == null)
        {
            return;
        }

        output.TagName = HtmlConstants.A;
        output.Attributes.Add("href", ActionUrl);

        var content = await output.GetChildContentAsync();

        var cssClass = IsSecondary ? "govuk-button govuk-button--secondary" : "govuk-button govuk-button--primary";
        var contentBuilder = new StringBuilder();
        contentBuilder.Append(CultureInfo.InvariantCulture, $"<button class=\"{cssClass}\" data-module=\"govuk-button\" type=\"button\">");
        contentBuilder.Append(content.GetContent());
        contentBuilder.Append("</button>");

        output.Content.SetHtmlContent(contentBuilder.ToString());
    }
}
