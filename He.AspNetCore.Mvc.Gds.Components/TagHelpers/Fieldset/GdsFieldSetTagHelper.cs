using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Fieldset
{
    /// <summary>
    /// Class GdsFieldSetTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsFieldSetTagHelper : TagHelper
    {

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
                output.TagName = HtmlConstants.FieldSet;

                if(Invalid)
                {
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkFieldSet} {CssConstants.GovUkFieldSetError}");
                }
                else
                {
                    TagConstruct.ConstructClass(output, CssConstants.GovUkFieldSet);
                }

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, null));
            }
        }
    }
}
