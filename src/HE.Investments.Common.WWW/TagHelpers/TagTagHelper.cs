using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class TagTagHelper : TagHelper
{
    public string Classes { get; set; }

    public TagColour? Colour { get; set; }

    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlConstants.Strong;

        var tagColor = (Colour ?? TagColour.Blue).ToString().ToLowerInvariant();
        var colorClass = $"govuk-tag--{tagColor}";
        TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {colorClass} {Classes}");

        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, Text));
    }
}
