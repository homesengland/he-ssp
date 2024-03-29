using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Labels
{
    /// <summary>
    /// Class GdsLabelTagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    public class GdsLabelTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets for.
        /// </summary>
        /// <value>For.</value>
        public string For { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public ControlSize? Size { get; set; }

        /// <summary>
        /// Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output != null)
            {
                output.TagName = HtmlConstants.Label;
                TagConstruct.ConstructGeneric(output, HtmlConstants.For, this.For);

                if (Size.HasValue)
                {
                    TagConstruct.ConstructClassForSize(output, CssConstants.GovUkLabel, Size.Value);
                }
                else
                {
                    TagConstruct.ConstructClass(output, CssConstants.GovUkLabel);
                }

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }
    }
}
