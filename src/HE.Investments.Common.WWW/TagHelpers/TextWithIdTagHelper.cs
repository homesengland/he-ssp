using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class TextWithIdTagHelper : TagHelper
{
    private readonly string _tagName;

    private readonly string _class;

    protected TextWithIdTagHelper(string tagName, string @class)
    {
        _tagName = tagName;
        _class = @class;
    }

    public string Id { get; set; }

    public string Text { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = _tagName;

        TagConstruct.ConstructId(output, Id);
        ConstructClass(output);

        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, Text));
    }

    protected virtual void ConstructClass(TagHelperOutput output)
    {
        TagConstruct.ConstructClass(output, _class);
    }
}
