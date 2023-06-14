using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace HE.InvestmentLoans.WWW.Controls
{
    /// <summary>
    /// Class GdsSummaryCardTitleWrapper.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryCardTitleWrapper : TextWithIdTagHelper
    {
        public GdsSummaryCardTitleWrapper() : base(HtmlConstants.Div, "govuk-summary-card__title-wrapper")
        {
        }
    }
}
