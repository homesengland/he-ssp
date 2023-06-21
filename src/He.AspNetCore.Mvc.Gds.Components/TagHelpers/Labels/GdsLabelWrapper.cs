using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Text;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Labels
{
    public class GdsLabelWrapper : StyledTag
    {
        public GdsLabelWrapper() : base(HtmlConstants.H1, "govuk-label-wrapper")
        {

        }
    }
}
