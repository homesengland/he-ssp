using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.SummaryCard
{
    /// <summary>
    /// Class GdsSummaryCardTagHelper.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryCardTagHelper : TextWithIdTagHelper
    {
        public GdsSummaryCardTagHelper() : base(HtmlConstants.Div, CssConstants.GovUkSummaryCard)
        {
        }
    }
}
