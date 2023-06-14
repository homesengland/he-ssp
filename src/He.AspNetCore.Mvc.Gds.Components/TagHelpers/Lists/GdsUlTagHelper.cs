using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Lists
{
    /// <summary>
    /// Class GdsUlTagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    [HtmlTargetElement("gds-ul", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class GdsUlTagHelper : TagHelper
    {
        /// <summary>
        /// Gets or sets the lists.
        /// </summary>
        /// <value>The lists.</value>
        [HtmlAttributeName(InputTagHelperConstants.Lists)]
#pragma warning disable CA1819 // Properties should not return arrays
        public string[] Lists { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        public string Text { get; set; }

        public bool NonBulletList { get; set; }
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
                output.TagName = HtmlConstants.Ul;

                if (NonBulletList)
                {
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkList}");
                }
                else
                {
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkList} {CssConstants.GovUkListBullet}");
                }

                if (Lists != null)
                {
                    output.Content.SetHtmlContent(TagConstruct.ConstructUlLists(this.Lists));
                }
                else
                {
                    output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
                }
            }
        }
    }
}
