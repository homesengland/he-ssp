using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Warning
{
    /// <summary>
    /// GDS Warning text.
    /// </summary>
    /// <seealso href="https://design-system.service.gov.uk/components/warning-text/" />
    public class GdsWarningTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets hidden text presented to assistive technologies. If not set, defaults to 'Warning'.
        /// </summary>
        public string AssistiveText { get; set; }

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
                output.TagName = HtmlConstants.Div;
                TagConstruct.ConstructClass(output, CssConstants.GovUkWarningText);

                var icon = new TagBuilder(HtmlConstants.Span);
                icon.AddCssClass(CssConstants.GovUkWarningTextIcon);
                icon.Attributes.Add(HtmlAttributes.AriaAttributes.Hidden, "true");
                icon.InnerHtml.Append("!");

                var text = new TagBuilder(HtmlConstants.Strong);
                text.AddCssClass(CssConstants.GovUkWarningTextText);

                var assistive = new TagBuilder(HtmlConstants.Span);
                assistive.AddCssClass(CssConstants.GovUkWarningTextAssistive);
                assistive.InnerHtml.Append(AssistiveText ?? "Warning");

                text.InnerHtml.AppendHtml(assistive);
                text.InnerHtml.Append(Text);

                output.Content.SetHtmlContent(string.Empty);
                output.Content.AppendHtml(icon);
                output.Content.AppendHtml(text);
            }
        }
    }
}
