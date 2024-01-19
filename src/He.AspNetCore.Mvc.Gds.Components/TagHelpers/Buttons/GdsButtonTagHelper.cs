using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Buttons
{
    /// <summary>
    /// Class GdsButtonTagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    public class GdsButtonTagHelper : TagHelper
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
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public bool Secondary { get; set; }

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
                output.TagName = HtmlConstants.Button;
                TagConstruct.ConstructId(output, this.Id);

                if (Secondary)
                {
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkButton} {CssConstants.GovUkSecondaryButton}");
                }
                else
                {
                    TagConstruct.ConstructClass(output, CssConstants.GovUkButton);
                }

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }
    }
}
