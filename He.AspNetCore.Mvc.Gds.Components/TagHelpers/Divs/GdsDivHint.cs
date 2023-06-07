using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Divs
{
    public class GdsDivHint : StyledTag
    {
        public GdsDivHint() : base(HtmlConstants.Div, "govuk-hint")
        {
        }
    }
}
