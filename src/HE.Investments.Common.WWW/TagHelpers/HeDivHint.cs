using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class HeDivHint : StyledTag
{
    public HeDivHint()
        : base(HtmlConstants.Div, CssConstants.GovUkHint)
    {
    }

    public string Id { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput? output)
    {
        TagConstruct.ConstructId(output, Id);
        base.Process(context, output);
    }
}
