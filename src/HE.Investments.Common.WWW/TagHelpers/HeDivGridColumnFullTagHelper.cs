using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class HeDivGridColumnFullTagHelper : TagHelper
{
    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput? output)
    {
        if (output == null)
        {
            return;
        }

        output.TagName = "div";
        TagConstruct.ConstructClass(output, "govuk-grid-column-full");
        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, Text));
    }
}
