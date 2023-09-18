using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using HE.InvestmentLoans.Contract.Application.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.InvestmentLoans.WWW.Views.Shared.Components;

public class ApplicationStatusTag : TextWithIdTagHelper
{
    public ApplicationStatusTag()
        : base(HtmlConstants.Strong, string.Empty)
    {
    }

    public ApplicationStatus ApplicationStatus { get; set; }

    protected override void ConstructClass(TagHelperOutput output)
    {
        Text = ApplicationStatus.ToString();
        TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag}");
    }
}
