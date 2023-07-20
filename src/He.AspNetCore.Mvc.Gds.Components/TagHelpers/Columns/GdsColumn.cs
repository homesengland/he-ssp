using System;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Columns
{
    public class GdsColumn : TagHelper
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        public ColumnWidth? Width { get; set; }
        /// <summary>
        /// process as an asynchronous operation.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <remarks>By default this calls into <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper.Process(Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContext,Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput)" />.</remarks>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = HtmlConstants.Div;

            TagConstruct.ConstructClass(output, CssFor(Width));

            output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
        }

        private string CssFor(ColumnWidth? width)
        {
            if (!width.HasValue)
            {
                return CssFor(ColumnWidth.Full);
            }

            return width switch
            {
                ColumnWidth.OneQuarter => "govuk-grid-column-one-quarter",
                ColumnWidth.ThreeQuarters => "govuk-grid-column-three-quarters",
                ColumnWidth.OneThirds => "govuk-grid-column-one-thirds",
                ColumnWidth.TwoThirds => "govuk-grid-column-two-thirds",
                ColumnWidth.OneHalf => "govuk-grid-column-one-half",
                ColumnWidth.Full => "govuk-grid-column-full",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
