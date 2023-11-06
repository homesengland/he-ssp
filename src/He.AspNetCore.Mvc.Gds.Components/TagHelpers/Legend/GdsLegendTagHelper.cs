using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Legend
{
    /// <summary>
    /// Class GdsLegendTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsLegendTagHelper : TagHelper
    {
        public ControlSize? Size { get; set; }

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
                output.TagName = HtmlConstants.Legend;

                if (Size.HasValue)
                {
                    TagConstruct.ConstructClassForSize(output, $"{CssConstants.GovUkFieldSetLegend}", Size.Value);
                }
                else
                {
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkFieldSetLegend}");
                }

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output));
            }
        }
    }
}
