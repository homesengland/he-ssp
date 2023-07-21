using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Spans
{
    /// <summary>
    /// Class GdsSpanVisuallyHidden.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSpanVisuallyHidden : TextTagHelper
    {
        public GdsSpanVisuallyHidden() : base(HtmlConstants.Span, CssConstants.GovVisuallyHidden)
        {
        }
    }
}
