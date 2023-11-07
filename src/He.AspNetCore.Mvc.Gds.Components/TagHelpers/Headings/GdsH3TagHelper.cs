using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Headings
{
    /// <summary>
    /// Class GdsH2TagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    public class GdsH3TagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The text.</value>
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
                output.TagName = HtmlConstants.H3;

                if (Size is null)
                {
                    TagConstruct.ConstructHeaderClass(output, ControlSize.M);
                }
                else
                {
                    TagConstruct.ConstructHeaderClass(output, Size.Value);
                }

                TagConstruct.ConstructId(output, this.Id);
                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }
    }
}
