using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.Enums
{
    public class GdsTagTagHelper : TextWithIdTagHelper
    {
        public TagColour Colour { get; set; }

        public GdsTagTagHelper() : base(HtmlConstants.Strong, "")
        {
        }

        protected override void ConstructClass(TagHelperOutput output)
        {
            TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {TagColourClass()}");
        }

        private string TagColourClass()
        {
            return $"govuk-tag--{Colour.ToString().ToLowerInvariant()}";
        }
    }
}
