using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Spans
{
    /// <summary>
    /// Class GdsSpanVisuallyHidden.
    /// Implements the <see cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />.
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Razor.TagHelpers.TagHelper" />
    public class GdsSpanVisuallyHidden : TextTagHelper
    {
        public GdsSpanVisuallyHidden() : base(HtmlConstants.Span, "govuk-visually-hidden")
        {
        }
    }
}
