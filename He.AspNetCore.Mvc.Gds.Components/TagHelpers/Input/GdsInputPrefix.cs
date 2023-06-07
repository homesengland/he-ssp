using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Input
{
    public class GdsInputPrefix : StyledTag
    {

        public GdsInputPrefix() : base(HtmlConstants.Div, "govuk-input__prefix")
        {
        }
    }
}
