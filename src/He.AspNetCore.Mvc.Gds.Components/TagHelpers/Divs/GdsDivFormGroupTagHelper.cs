using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Divs
{
    /// <summary>
    /// Class GdsDivFormGroupTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsDivFormGroupTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public bool Invalid { get; set; }

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

                if (Invalid)
                {
                    TagConstruct.ChangeClassToError(output, CssConstants.GovUkFormGroup);
                }
                else
                {
                    TagConstruct.ConstructClass(output, CssConstants.GovUkFormGroup);
                }
                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }
    }
}
