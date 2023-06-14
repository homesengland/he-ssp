using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Summary.V2
{
    /// <summary>
    /// Class GdsSummaryListValueV2TagHelper.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryListValueV2TagHelper : TextWithIdTagHelper
    {
        public GdsSummaryListValueV2TagHelper() : base(HtmlConstants.DD, "govuk-summary-list__value")
        {
        }
    }
}
