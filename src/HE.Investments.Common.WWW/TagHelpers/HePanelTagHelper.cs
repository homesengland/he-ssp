using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class HePanelTagHelper : TagHelper
{
    public string Title { get; set; }

    public string SubTitle { get; set; }

    public string Reference { get; set; }

    public bool Confirmation { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlConstants.Div;
        output.TagMode = TagMode.StartTagAndEndTag;
        TagConstruct.ConstructClass(output, HeCssConstants.GovUkPanel);

        if (Confirmation)
        {
            TagConstruct.ConstructClass(output, HeCssConstants.GovUkConfirmationPanel);
        }

        var header = new TagBuilder(HtmlConstants.H1);
        header.AddCssClass(HeCssConstants.GovUkPanelTitle);
        header.InnerHtml.Append(Title);

        var lineBreak = new TagBuilder("br") { TagRenderMode = TagRenderMode.SelfClosing };

        var reference = new TagBuilder(HtmlConstants.Strong);
        reference.InnerHtml.Append(Reference);

        var body = new TagBuilder(HtmlConstants.Div);
        body.AddCssClass(HeCssConstants.GovUkPanelBody);

        body.InnerHtml.Append(SubTitle);
        body.InnerHtml.AppendHtml(lineBreak);
        body.InnerHtml.AppendHtml(reference);

        output.Content.SetHtmlContent(string.Empty);
        output.Content.AppendHtml(header);
        output.Content.AppendHtml(body);
    }
}
