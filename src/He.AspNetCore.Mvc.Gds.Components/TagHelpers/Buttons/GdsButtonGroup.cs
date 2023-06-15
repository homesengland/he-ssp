using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Buttons
{
    public class GdsButtonGroup : StyledTag
    {
        public GdsButtonGroup() : base(HtmlConstants.Div, CssConstants.GovUkButtonGroup)
        {
        }
    }
}
