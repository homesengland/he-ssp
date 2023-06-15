using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Errors
{
    public class GdsErrorSummaryMessage : TextWithIdTagHelper
    {
        public GdsErrorSummaryMessage() : base(HtmlConstants.Div, "govuk-error-message")
        {
        }
    }

}
