using System.Collections.Generic;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Lists
{
    /// <summary>
    /// Class GdsLiTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsLiTagHelper : TagHelper
    {
        /// <summary>
        /// Gets the lists.
        /// </summary>
        /// <value>The lists.</value>
        public List<string> Lists { get; }


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
                output.TagName = HtmlConstants.Li;


                if (Lists != null)
                {
                    output.Content.SetHtmlContent(TagConstruct.ConstructUlLists(this.Lists.ToArray()));
                }
                else
                {
                    output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output));
                }
            }
        }
    }
}
