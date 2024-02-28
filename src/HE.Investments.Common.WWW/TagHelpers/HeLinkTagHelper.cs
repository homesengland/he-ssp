using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class HeLinkTagHelper : TagHelper
{
    public string Href { get; set; }

    public string Text { get; set; }

    public string? Target { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlConstants.A;
        TagConstruct.ConstructClass(output, CssConstants.GovUkLink);
        TagConstruct.ConstructHref(output, Href);

        if (Target.IsProvided())
        {
            TagConstruct.ConstructGenericAttribute(output, "target", Target);
        }

        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, Text).Trim());
    }
}
