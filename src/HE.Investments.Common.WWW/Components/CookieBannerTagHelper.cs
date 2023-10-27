using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.Components;

public class CookieBannerTagHelper : TagHelper
{
    public string CookiesPageUrl { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (output == null)
        {
            return;
        }

        output.TagName = HtmlConstants.Div;

        TagConstruct.ConstructClass(output, "govuk-cookie-banner govuk-!-display-none");
        output.Attributes.Add("role", "region");
        output.Attributes.Add("data-nosnippet", null);
        output.Attributes.Add("aria-label", $"Can we store additional cookies?");

        var content = ResourceReader.Read("CookieBannerTemplate.html")
            .Replace("[CookiesPage]", CookiesPageUrl);

        output.Content.SetHtmlContent(content);
    }
}
