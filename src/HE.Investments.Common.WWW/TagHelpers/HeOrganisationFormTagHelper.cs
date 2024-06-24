using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class HeOrganisationFormTagHelper : FormTagHelper
{
    public HeOrganisationFormTagHelper(IHtmlGenerator generator)
        : base(generator)
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var organisationId = ViewContext.HttpContext.GetOrganisationIdFromRoute()?.ToString();
        RouteValues["organisationId"] = organisationId;

        output.TagName = "form";
        base.Process(context, output);
    }
}
