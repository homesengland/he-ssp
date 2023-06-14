using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.InvestmentLoans.WWW.Controls
{
    /// <summary>
    /// Class GdsSummaryCardTitle.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSummaryCardTitle : TextWithIdTagHelper
    {
        public GdsSummaryCardTitle() : base(HtmlConstants.H2, "govuk-summary-card__title")
        {
        }
    }
}
