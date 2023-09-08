using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Divs
{
    public class GdsDivHint : StyledTag
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        public GdsDivHint() : base(HtmlConstants.Div, CssConstants.GovUkHint)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagConstruct.ConstructId(output, Id);
            base.Process(context, output);
        }
    }
}
