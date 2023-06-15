using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Details
{
    public class GdsDetailsSummary : TagHelper
    {
        public string Label { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (output != null)
            {
                output.TagName = HtmlConstants.Details;
                TagConstruct.ConstructClass(output, CssConstants.GovUkDetails);

                var contentBuilder = new StringBuilder();

                contentBuilder.Append($"<summary class=\"{CssConstants.GovUkDetails}__summary\">");
                contentBuilder.Append($"<span class=\"{CssConstants.GovUkDetails}__summary-text\">");
                contentBuilder.Append(Label);
                contentBuilder.Append($"</span>");
                contentBuilder.Append($"</summary>");
                contentBuilder.Append($"<div class=\"{CssConstants.GovUkDetails}__text\">");
                contentBuilder.Append(TagConstruct.ConstructSetHtml(output));
                contentBuilder.Append($"</div>");
                contentBuilder.Append($"</details>");

                output.Content.SetHtmlContent(contentBuilder.ToString());
            }
        }
    }
}
