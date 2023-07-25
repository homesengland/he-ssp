using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Errors
{
    public class GdsErrorSummaryTitle : TextWithIdTagHelper
    {
        public GdsErrorSummaryTitle() : base(HtmlConstants.H2, CssConstants.GovUkErrorSummaryTitle)
        {
        }
    }

}
