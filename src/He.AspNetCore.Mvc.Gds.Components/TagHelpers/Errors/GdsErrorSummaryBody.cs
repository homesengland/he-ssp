using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Errors
{
    public class GdsErrorSummaryBody : TextWithIdTagHelper
    {
        public GdsErrorSummaryBody() : base(HtmlConstants.Div, "govuk-error-summary__body")
        {
        }
    }

}
