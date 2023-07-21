using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Summary.V2
{
    /// <summary>
    /// Class GdsSummaryListActionsV2TagHelper.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryListActionsV2TagHelper : TextWithIdTagHelper
    {
        public GdsSummaryListActionsV2TagHelper() : base(HtmlConstants.DD, CssConstants.GovUkSummaryListActions)
        {
        }
    }
}
