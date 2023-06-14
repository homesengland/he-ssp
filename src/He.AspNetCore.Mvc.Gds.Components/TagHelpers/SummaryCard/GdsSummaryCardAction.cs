using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.SummaryCard
{
    public class GdsSummaryCardAction : StyledTag
    {
        public GdsSummaryCardAction() : base(HtmlConstants.Li, "govuk-summary-card__action")
        {
        }
    }
}
