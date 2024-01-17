using System.Globalization;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class LinkButtonTagHelper : TagHelper
{
    public string ActionUrl { get; set; }

    public bool IsSecondary { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsWarning { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlConstants.A;
        output.Attributes.Add("href", ActionUrl);

        TagConstruct.ConstructClass(output, "govuk-button");
        TagConstructExtensions.ConstructClass(output, "govuk-button--secondary", IsSecondary);
        TagConstructExtensions.ConstructClass(output, "govuk-button--disabled", IsDisabled);
        TagConstructExtensions.ConstructClass(output, "govuk-button--warning", IsWarning);

        if (IsDisabled)
        {
            TagConstruct.ConstructGenericAttribute(output, "disabled", "disabled");
            TagConstruct.ConstructGenericAttribute(output, "aria-disabled", "true");
        }

        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output));
        return Task.CompletedTask;
    }
}
