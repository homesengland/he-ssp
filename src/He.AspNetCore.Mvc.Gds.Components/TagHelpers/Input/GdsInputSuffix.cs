using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Input
{
    public class GdsInputSuffix : StyledTag
    {
        public GdsInputSuffix() : base(HtmlConstants.Div, CssConstants.GovUkInputSuffix)
        {
        }
    }
}
