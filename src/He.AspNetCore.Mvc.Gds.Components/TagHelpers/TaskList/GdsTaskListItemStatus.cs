using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Enums;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList
{
    /// <summary>
    /// Class GdsSpanAppListTaskCompletedTagHelper.
    /// Implements the <see cref="TagHelper" />.
    /// </summary>
    /// <seealso cref="TagHelper" />
    public class GdsTaskListItemStatus : TextWithIdTagHelper
    {
        public GdsTaskListItemStatusText Status { get; set; }

        public GdsTaskListItemStatus() : base(HtmlConstants.Strong, "")
        {
        }

        protected override void ConstructClass(TagHelperOutput output)
        {
            if (Status == GdsTaskListItemStatusText.Completed)
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {CssConstants.GovUkAppTaskListTag} {CssConstants.GovUkAppTaskListTaskCompleted}");
            }
            else if (Status == GdsTaskListItemStatusText.InProgress)
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {CssConstants.GovUkAppTaskListTag} govuk-tag--blue");
            }
            else
            {
                TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {CssConstants.GovUkAppTaskListTag} govuk-tag--grey");
            }
        }
    }
}
