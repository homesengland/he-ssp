using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.SummaryCard
{
    public class GdsSummaryCardContent : StyledTag
    {
        public GdsSummaryCardContent() : base(HtmlConstants.Div, CssConstants.GovUkSummaryCardContent)
        {
        }
    }
}
