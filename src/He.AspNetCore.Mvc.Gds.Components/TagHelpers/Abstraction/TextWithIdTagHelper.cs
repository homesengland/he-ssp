using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction
{
    public class TextWithIdTagHelper : TagHelper
    {
        private readonly string _tagName;
#pragma warning disable IDE1006 // Naming Styles
        protected string _class;
#pragma warning restore IDE1006 // Naming Styles

        public string Id { get; set; }
        public string Text { get; set; }

        public TextWithIdTagHelper(string tagName, string @class)
        {
            _tagName = tagName;
            _class = @class;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output != null)
            {
                output.TagName = _tagName;

                TagConstruct.ConstructId(output, this.Id);
                ConstructClass(output);

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, this.Text));
            }
        }

        protected virtual void ConstructClass(TagHelperOutput output)
        {
            TagConstruct.ConstructClass(output, _class);
        }
    }
}
