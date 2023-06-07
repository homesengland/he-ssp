using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Divs
{
    /// <summary>
    /// Class GdsDivDateInput.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsDivDateInputTagHelper : TextTagHelper
    {
        public GdsDivDateInputTagHelper() : base(HtmlConstants.Div, "govuk-date-input")
        {
        }
    }
}
