using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction
{
    public abstract class TextTagHelper : TagHelper
    {
        private readonly string _tagName;
        private readonly string _class;

        public string Text { get; set; }

        protected TextTagHelper(string tagName, string @class)
        {
            _tagName = tagName;
            _class = @class;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output != null)
            {
                output.TagName = _tagName;
                TagConstruct.ConstructClass(output, _class);
                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }
    }
}
