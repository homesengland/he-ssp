using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.SummaryCard
{
    public class GdsSummaryCardActions : TextWithIdTagHelper
    {
        public GdsSummaryCardActions() : base(HtmlConstants.Ul, CssConstants.GovUkSummaryCardActions)
        {
        }
    }
}
