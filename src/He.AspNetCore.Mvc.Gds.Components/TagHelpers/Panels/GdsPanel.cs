using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Panels
{
    public class GdsPanel : TagHelper
    {
        public bool Confirmation { get; set; }

        public string Title { get; set; }

        public GdsPanel()
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = HtmlConstants.Div;

            if(Confirmation)
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkPanel} {CssConstants.GovUkConfirmationPanel}");
            }
            else
            {
                TagConstruct.ConstructClass(output, CssConstants.GovUkPanel);
            }


            var contentBuilder = new StringBuilder();

            contentBuilder.Append($"<h1 class=\"{CssConstants.GovUkPanelTitle}\">{Title}</h1>");


            contentBuilder.Append($"<div class=\"{CssConstants.GovUkPanelBody}\">{TagConstruct.ConstructSetHtml(output)}</div>");

            output.Content.SetHtmlContent(contentBuilder.ToString());
        }
    }
}
