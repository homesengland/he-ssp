using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Errors
{
    public class GdsErrorSummary : TextWithIdTagHelper
    {
        public GdsErrorSummary() : base(HtmlConstants.Div, "govuk-error-summary")
        {
        }
    }
}
