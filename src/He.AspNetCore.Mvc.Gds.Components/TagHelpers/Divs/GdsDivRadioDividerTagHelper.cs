using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Divs
{
    /// <summary>
    /// Class GdsDivRadioDividerTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsDivRadioDividerTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output != null)
            {
                output.TagName = HtmlConstants.Div;
                TagConstruct.ConstructClass(output, CssConstants.GovUkRadioDivider);
                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }
    }
}
