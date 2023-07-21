using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Labels
{
    public class GdsLabelWrapper : StyledTag
    {
        public GdsLabelWrapper() : base(HtmlConstants.H1, CssConstants.GovUkLabelWrapper)
        {

        }
    }
}
