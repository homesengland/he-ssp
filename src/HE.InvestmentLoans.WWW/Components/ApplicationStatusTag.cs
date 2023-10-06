using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.Extensions;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Abstraction;
using He.AspNetCore.Mvc.Gds.Components.TagHelpers.Tags;
using HE.InvestmentLoans.Contract.Application.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.InvestmentLoans.WWW.Components;

public class ApplicationStatusTag : TextWithIdTagHelper
{
    public ApplicationStatusTag()
        : base(HtmlConstants.Strong, string.Empty)
    {
    }

    public ApplicationStatus ApplicationStatus { get; set; }

    public string AdditionalClasses { get; set; }

    protected override void ConstructClass(TagHelperOutput output)
    {
        Text = ApplicationStatus.GetDescription();
        TagConstruct.ConstructClass(output, $"{CssConstants.GovUkTag} {TagColourClass(ApplicationStatus)} {AdditionalClasses}");
    }

    private string TagColourClass(ApplicationStatus applicationStatus)
    {
        return $"govuk-tag--{GetColorBaseOnStatus(applicationStatus).ToString().ToLowerInvariant()}";
    }

    private TagColour GetColorBaseOnStatus(ApplicationStatus applicationStatus)
    {
        return ApplicationStatus switch
        {
            ApplicationStatus.New => TagColour.Blue,
            ApplicationStatus.Draft => TagColour.Blue,
            ApplicationStatus.CashflowRequested => TagColour.Blue,
            ApplicationStatus.ReferredBackToApplicant => TagColour.Blue,
            ApplicationStatus.HoldRequested => TagColour.Grey,
            ApplicationStatus.OnHold => TagColour.Grey,
            ApplicationStatus.UnderReview => TagColour.Orange,
            ApplicationStatus.ApplicationUnderReview => TagColour.Orange,
            ApplicationStatus.InDueDiligence => TagColour.Orange,
            ApplicationStatus.CashflowUnderReview => TagColour.Orange,
            ApplicationStatus.ApplicationSubmitted => TagColour.Green,
            ApplicationStatus.SentForApproval => TagColour.Green,
            ApplicationStatus.ApprovedSubjectToContract => TagColour.Green,
            ApplicationStatus.ApprovedSubjectToDueDiligence => TagColour.Green,
            ApplicationStatus.ContractSigned => TagColour.Green,
            ApplicationStatus.CpsSatisfied => TagColour.Green,
            ApplicationStatus.LoanAvailable => TagColour.Green,
            ApplicationStatus.Withdrawn => TagColour.Red,
            ApplicationStatus.ApplicationDeclined => TagColour.Red,
            ApplicationStatus.NA => TagColour.Grey,
            _ => TagColour.Grey,
        };
    }
}
