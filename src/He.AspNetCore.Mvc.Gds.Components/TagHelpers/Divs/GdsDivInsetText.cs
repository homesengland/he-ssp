using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Divs
{
    public class GdsDivInsetText : StyledTag
    {
        public GdsDivInsetText() : base(HtmlConstants.Div, "govuk-inset-text")
        {
        }
    }
}
