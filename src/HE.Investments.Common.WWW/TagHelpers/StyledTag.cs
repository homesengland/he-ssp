using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public abstract class StyledTag : TagHelper
{
    private readonly string _tagName;

    private readonly string _class;

    protected StyledTag(string tag, string @class)
    {
        _tagName = tag;
        _class = @class;
    }

    public override void Process(TagHelperContext context, TagHelperOutput? output)
    {
        if (output != null)
        {
            output.TagName = _tagName;
            TagConstruct.ConstructClass(output, _class);
            output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output));
        }
    }
}
