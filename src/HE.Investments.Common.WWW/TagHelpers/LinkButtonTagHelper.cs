using System.Globalization;
using System.Text;
using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Gds;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

public class LinkButtonTagHelper : TagHelper
{
    public string ActionUrl { get; set; }

    public bool IsDisabled { get; set; }

    public ButtonType ButtonType { get; set; }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = HtmlConstants.A;
        output.Attributes.Add("href", ActionUrl);

        TagConstruct.ConstructClass(output, "govuk-button");
        TagConstructExtensions.ConstructClass(output, AssignButtonType());

        if (IsDisabled)
        {
            TagConstructExtensions.ConstructClass(output, "govuk-button--disabled", IsDisabled);
            TagConstruct.ConstructGenericAttribute(output, "disabled", "disabled");
            TagConstruct.ConstructGenericAttribute(output, "aria-disabled", "true");
        }

        output.Content.SetHtmlContent(TagConstruct.ConstructSetHtml(output));
        return Task.CompletedTask;
    }

    private string AssignButtonType() => ButtonType switch
    {
        ButtonType.Secondary => "govuk-button--secondary",
        ButtonType.Warning => "govuk-button--warning",
        _ => string.Empty,
    };
}
