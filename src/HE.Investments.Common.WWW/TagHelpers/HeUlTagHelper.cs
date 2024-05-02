using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

[HtmlTargetElement("he-ul", TagStructure = TagStructure.NormalOrSelfClosing)]
public class HeUlTagHelper : TagHelper
{
    public IList<string> Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";
        TagConstruct.ConstructClass(output, "govuk-list govuk-list govuk-list--bullet");
        output.Content.SetHtmlContent(TagConstruct.ConstructUlLists([.. Items]));
    }
}
