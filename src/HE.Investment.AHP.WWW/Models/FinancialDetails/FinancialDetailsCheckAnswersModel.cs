using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.WWW.Components.SectionSummary;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsCheckAnswersModel : FinancialDetailsBaseModel
{
    public FinancialDetailsCheckAnswersModel()
    {
    }

    public FinancialDetailsCheckAnswersModel(
        Guid applicationId,
        string applicationName,
        SectionSummaryViewModel landValueSummary,
        SectionSummaryViewModel costsSummary,
        SectionSummaryViewModel contributionsSummary,
        IsSectionCompleted isSectionCompleted)
        : base(applicationId, applicationName)
    {
        LandValueSummary = landValueSummary;
        CostsSummary = costsSummary;
        ContributionsSummary = contributionsSummary;
        IsSectionCompleted = isSectionCompleted;
    }

    [ValidateNever]
    public SectionSummaryViewModel LandValueSummary { get; set; }

    [ValidateNever]
    public SectionSummaryViewModel CostsSummary { get; set; }

    [ValidateNever]
    public SectionSummaryViewModel ContributionsSummary { get; set; }

    [ValidateNever]
    public bool? CostsAndFunding { get; set; }

    public IsSectionCompleted IsSectionCompleted { get; set; }
}
