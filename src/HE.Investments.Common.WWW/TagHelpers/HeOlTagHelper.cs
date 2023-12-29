using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

[HtmlTargetElement("he-ol", TagStructure = TagStructure.NormalOrSelfClosing)]
public class HeOlTagHelper : TagHelper
{
    public IList<string> Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ol";
        TagConstruct.ConstructClass(output, "govuk-list govuk-list--number");
        output.Content.SetHtmlContent(TagConstruct.ConstructUlLists(Items.ToArray()));
    }
}
