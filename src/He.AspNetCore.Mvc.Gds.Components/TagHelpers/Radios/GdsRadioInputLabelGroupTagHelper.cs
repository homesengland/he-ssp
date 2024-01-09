using System.Collections.Generic;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Models;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Radios
{
    /// <summary>
    /// GdsRadioInputLabelGroupTagHelper radio.
    /// </summary>
    public class GdsRadioInputLabelGroupTagHelper : TagHelper
    {
        /// <summary>
        /// Gets the radio input group.
        /// </summary>
        public List<GdsRadioInputGroup> RadioInputGroup { get; }

        /// <summary>
        /// Synchronously executes the <see cref="TagHelper" /> with the given <paramref name="context" /> and
        /// <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.RadioInputGroup != null && output != null)
            {
                output.TagName = HtmlConstants.Div;
                TagConstruct.ConstructClass(output, CssConstants.GovUkRadios);
                var sb = string.Empty;
                foreach (var radioInputGroup in this.RadioInputGroup)
                {
                    sb += TagConstruct.ConstructRadioInputLabel(
                        radioInputGroup.Id,
                        radioInputGroup.Name,
                        radioInputGroup.Value,
                        radioInputGroup.LabelText,
                        radioInputGroup.For,
                        radioInputGroup.Checked,
                        radioInputGroup.Divider,
                        radioInputGroup.DividerText,
                        radioInputGroup.HintText);
                }

                output.Content.SetHtmlContent(sb);
            }
        }
    }
}
