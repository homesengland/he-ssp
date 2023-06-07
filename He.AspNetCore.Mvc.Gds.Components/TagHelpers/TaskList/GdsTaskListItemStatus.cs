using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Strong
{
    /// <summary>
    /// Class GdsSpanAppListTaskCompletedTagHelper.
    /// Implements the <see cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsTaskListItemStatus : TextWithIdTagHelper
    {
        public bool Completed { get; set; }

        public GdsTaskListItemStatus() : base(HtmlConstants.Strong, "")
        {
        }

        protected override void ConstructClass(TagHelperOutput output)
        {
            if (Completed)
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {CssConstants.GovUkAppTaskListTag} {CssConstants.GovUkAppTaskListTaskCompleted}");
            }
            else
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {CssConstants.GovUkAppTaskListTag} govuk-tag--grey");
            }
        }
    }
}
