using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.AspNetCore.Mvc.Gds.Components.TagHelpers.Errors
{
    public class GdsErrorFormGroup : TagHelper
    {
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output != null)
            {
                output.TagName = HtmlConstants.Div;

                var (hasError, _) = ViewContext.GetErrorFrom(For);

                if (hasError)
                {
                    TagConstruct.ConstructClass(output, $"{CssConstants.GovUkFormGroup} {CssConstants.GovUkFieldSetError}");
                }
                else
                {
                    TagConstruct.ConstructClass(output, CssConstants.GovUkFormGroup);
                }

                output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output, null));
            }
        }
    }

}
