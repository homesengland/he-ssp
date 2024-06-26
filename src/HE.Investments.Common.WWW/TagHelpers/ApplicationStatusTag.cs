using He.AspNetCore.Mvc.Gds.Components.Constants;
using He.AspNetCore.Mvc.Gds.Components.TagConstructs;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace HE.Investments.Common.WWW.TagHelpers;

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
        return applicationStatus switch
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
            ApplicationStatus.AwaitingCpSatisfaction => TagColour.Green,
            ApplicationStatus.ConditionsSatisfied => TagColour.Green,
            ApplicationStatus.LoanAvailable => TagColour.Green,
            ApplicationStatus.Withdrawn => TagColour.Red,
            ApplicationStatus.ApplicationDeclined => TagColour.Red,
            ApplicationStatus.NA => TagColour.Grey,
            ApplicationStatus.Approved => TagColour.Green,
            ApplicationStatus.RequestedEditing => TagColour.Grey,
            ApplicationStatus.Rejected => TagColour.Red,
            ApplicationStatus.Deleted => TagColour.Red,
            _ => TagColour.Grey,
        };
    }
}
