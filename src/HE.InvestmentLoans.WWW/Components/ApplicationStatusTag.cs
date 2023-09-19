using He.AspNetCore.Mvc.Gds.Components.Constants;
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
        Text = ApplicationStatus.ToString();
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
            ApplicationStatus.New => TagColour.Green,
            ApplicationStatus.Draft => TagColour.Green,
            ApplicationStatus.Submitted => TagColour.Purple,
            ApplicationStatus.ApprovedSubjectToContract => TagColour.Purple,
            ApplicationStatus.ApprovedSubjectToDueDiligence => TagColour.Purple,
            ApplicationStatus.CashflowRequested => TagColour.Turquoise,
            ApplicationStatus.ReferredBackToApplicant => TagColour.Turquoise,
            ApplicationStatus.NA => TagColour.Grey,
            ApplicationStatus.UnderReview => TagColour.Blue,
            ApplicationStatus.CashflowUnderReview => TagColour.Blue,
            ApplicationStatus.SentForApproval => TagColour.Blue,
            ApplicationStatus.ContractSigned => TagColour.Blue,
            ApplicationStatus.CspSatisfied => TagColour.Blue,
            ApplicationStatus.LoanAvailable => TagColour.Blue,
            ApplicationStatus.Withdrawn => TagColour.Yellow,
            ApplicationStatus.HoldRequested => TagColour.Yellow,
            ApplicationStatus.OnHold => TagColour.Yellow,
            ApplicationStatus.ApplicationDeclined => TagColour.Orange,
            ApplicationStatus.InDueDiligence => TagColour.Orange,
            _ => throw new ArgumentOutOfRangeException(nameof(applicationStatus)),
        };
    }
}
