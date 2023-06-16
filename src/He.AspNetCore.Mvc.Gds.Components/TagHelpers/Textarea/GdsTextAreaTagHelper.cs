using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Interfaces;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Textarea
{
    /// <summary>
    /// Class GdsTextAreaTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsTextAreaTagHelper : InputTagHelper
    {

        public GdsTextAreaTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

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
        /// Gets or sets the rows.
        /// </summary>
        /// <value>The rows.</value>
        public string Rows { get; set; }

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
                output.TagName = HtmlConstants.Textarea;
                TagConstruct.ConstructId(output, Id);

                if(For != null)
                {
                    var contentBuilder = new StringBuilder();

                    var fullHtmlFieldName = IGdsFormGroupTagHelper.GetFullHtmlFieldName(ViewContext, For.Name);
                    var propertyInError = IGdsFormGroupTagHelper.IsPropertyInError(ViewContext, fullHtmlFieldName);

                    if (propertyInError.isPropertyInError)
                    {
                        output.TagName = HtmlConstants.Div;

                        contentBuilder.Append($"<p id=\"contact-by-email-error\" class=\"govuk-error-message\">  " +
                                                $"<span class=\"govuk-visually-hidden\">Error:</span> {propertyInError.entry.Errors.First().ErrorMessage}" +
                                            $"</p>");

                        contentBuilder.Append($"<textarea class=\"govuk-textarea govuk-textarea--error\" rows=\"{Rows ?? "5"}\" id=\"{For.Name}\" name=\"{For.Name}\">{For.Model}</textarea>");

                        output.Content.SetHtmlContent(contentBuilder.ToString());
                        return;
                    }
                    else
                    {

                        TagConstruct.ConstructName(output, For.Name);
                        TagConstruct.ConstructClass(output, $"govuk-textarea");

                        TagConstruct.ConstructGeneric(output, HtmlConstants.Rows, Rows);

                        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, (string)For.Model));
                    }

                }
                else
                {
                    TagConstruct.ConstructName(output, Name);
                    TagConstruct.ConstructValue(output, Value);
                    TagConstruct.ConstructClass(output, $"govuk-textarea");
                    TagConstruct.ConstructGeneric(output, HtmlConstants.Rows, Rows);

                    output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, Text));
                }
            }
        }
    }
}
