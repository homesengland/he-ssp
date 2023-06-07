using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Errors
{
    public class GdsErrorSummaryList : TextWithIdTagHelper
    {
        public GdsErrorSummaryList() : base(HtmlConstants.Ul, "govuk-list govuk-error-summary__list")
        {
        }
    }

}
