using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Fieldset
{
    public class GdsFieldsetHeading : StyledTag
    {
        public GdsFieldsetHeading() : base(HtmlConstants.H1, CssConstants.GovUkFieldSetHeading)
        {
        }
    }
}
