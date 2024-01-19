using System.Collections.Generic;
using System.Linq;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.Net.Mime.MediaTypeNames;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios
{
    /// <summary>
    /// Class GdsRadioYesNoTagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    public class GdsCustomRadioTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GdsRadioYesNoTagHelper"/> is inline.
        /// </summary>
        /// <value><c>true</c> if inline; otherwise, <c>false</c>.</value>
        public bool Inline { get; set; }


        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public IEnumerable<SelectListItem> RadioItems { get; set; }
        public IEnumerable<string> ConditionalInputIds { get; set; }
        public IEnumerable<string> ConditionalInputLabels { get; set; }
        public IEnumerable<string> ConditionalInputNames { get; set; }
        public IEnumerable<string> ConditionalInputValues { get; set; }

        public IEnumerable<string> RadioHints { get; set; }

        public bool IsConditionalInputInvalid { get; set; }

        public string ConditionalInputError { get; set; }

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

                var css = CssConstants.GovUkRadios;
                if (Inline)
                {
                    css += $" {CssConstants.GovUkRadiosInline}";
                }

                TagConstruct.ConstructClass(output, css);

                var radioItems = RadioItems.ToArray();
                var conditionalInputIds = ConditionalInputIds?.ToArray() ?? new string[0];
                var conditionalInputLabels = ConditionalInputLabels?.ToArray() ?? new string[0];
                var conditionalInputNames = ConditionalInputNames?.ToArray() ?? new string[0];
                var conditionalInputValues = ConditionalInputValues?.ToArray() ?? new string[0];
                var hints = RadioHints?.ToArray() ?? new string[0];

                var sb = new StringBuilder();
                for (var i = 0; i < RadioItems.Count(); i++)
                {
                    var item = radioItems[i];
                    var selectRadio = item.Value == Value;
                    var text = item.Text;
                    var value = item.Value;

                    var id = i == 0 ? For.Name : $"{For.Name}-{i}";
                    var builder = TagConstruct.CreateRadio()
                            .AsRadio(id, For.Name, value)
                            .WithLabel(text, id);

                    if (conditionalInputLabels.Length > i && conditionalInputValues.Length > i)
                    {
                        builder.WithConditionalInput(conditionalInputIds[i], conditionalInputLabels[i], conditionalInputNames[i], conditionalInputValues[i]);
                    }
                    else if (conditionalInputLabels.Length > i)
                    {
                        builder.WithConditionalInput(conditionalInputIds[i], conditionalInputLabels[i], conditionalInputNames[i]);
                    }

                    if (IsConditionalInputInvalid && selectRadio)
                    {
                        builder.WithConditionalErrorMessage(ConditionalInputError);
                    }

                    if (hints.Length > i)
                    {
                        builder.WithHint(hints[i]);
                    }

                    if (selectRadio)
                    {
                        builder.ThatIsChecked();
                    }

                    sb.Append(builder.Build());
                }

                output.Content.SetHtmlContent(sb.ToString());
            }
        }
    }
}
