using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Summary.V2
{
    /// <summary>
    /// Class GdsSummaryListKeyV2TagHelper.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryListKeyV2TagHelper : TextWithIdTagHelper
    {
        public GdsSummaryListKeyV2TagHelper() : base(HtmlConstants.Dt, CssConstants.GovUkSummaryListKey)
        {
        }
    }
}
