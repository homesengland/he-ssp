using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Summary.V2
{
    /// <summary>
    /// Class GdsSummaryListRowV2TagHelper.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryListRowV2TagHelper : TextWithIdTagHelper
    {
        public GdsSummaryListRowV2TagHelper() : base(HtmlConstants.Div, "govuk-summary-list__row")
        {
        }
    }
}
