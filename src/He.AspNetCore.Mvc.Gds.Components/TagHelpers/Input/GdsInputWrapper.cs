using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Input
{
    public class GdsInputWrapper : StyledTag
    {
        public GdsInputWrapper() : base(HtmlConstants.Div, CssConstants.GovUkInputWrapper)
        {
        }
    }
}
