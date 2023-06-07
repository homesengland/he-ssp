using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.TaskList
{
    public abstract class StyledTag : TagHelper
    {
        private string _tagName;
        private string _class;

        public StyledTag(string tag, string @class)
        {
            _tagName = tag;
            _class = @class;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if ((object)output != (object)null)
            {
                output.TagName = _tagName;
                TagConstruct.ConstructClass(output, _class);
                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, null));
            }
        }
    }

    public class GdsTaskListSection : StyledTag
    {
        public GdsTaskListSection() : base(HtmlConstants.H2, "app-task-list__section")
        {
        }
    }
}
