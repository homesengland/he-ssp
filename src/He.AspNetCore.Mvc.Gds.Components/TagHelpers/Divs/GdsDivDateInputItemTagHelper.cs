using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace HE.InvestmentLoans.WWW.Controls
{
    /// <summary>
    /// Class GdsDivDateInputItem.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsDivDateInputItemTagHelper : TextTagHelper
    {
        public GdsDivDateInputItemTagHelper() : base(HtmlConstants.Div, "govuk-date-input__item")
        {
        }
    }
}
