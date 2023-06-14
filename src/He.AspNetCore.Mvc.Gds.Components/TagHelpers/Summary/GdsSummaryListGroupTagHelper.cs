using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Summary
{
    /// <summary>
    /// Class GdsSummaryListGroupTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryListGroupTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>The href.</value>
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the link text.
        /// </summary>
        /// <value>The link text.</value>
        public string LinkText { get; set; }

        /// <summary>
        /// Gets or sets the visually hidden.
        /// </summary>
        /// <value>The visually hidden.</value>
        public string VisuallyHidden { get; set; }

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
                TagConstruct.ConstructClass(output, CssConstants.GovUkSummaryListRow);
                TagConstruct.ConstructId(output, this.Id);
                var outputHtml = TagConstruct.ConstructSummaryListGroup(this.Key, this.Value, this.Href, this.LinkText, this.VisuallyHidden);
                output.Content.SetHtmlContent(outputHtml);
            }
        }
    }
}
