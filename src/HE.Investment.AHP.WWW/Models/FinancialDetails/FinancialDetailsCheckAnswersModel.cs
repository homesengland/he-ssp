using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsCheckAnswersModel : FinancialDetailsBaseModel, IEditableViewModel
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
        IsSectionCompleted isSectionCompleted,
        bool isEditable,
        bool isApplicationLocked)
        : base(applicationId, applicationName)
    {
        LandValueSummary = landValueSummary;
        CostsSummary = costsSummary;
        ContributionsSummary = contributionsSummary;
        IsSectionCompleted = isSectionCompleted;
        IsEditable = isEditable;
        IsApplicationLocked = isApplicationLocked;
    }

    [ValidateNever]
    public SectionSummaryViewModel LandValueSummary { get; set; }

    [ValidateNever]
    public SectionSummaryViewModel CostsSummary { get; set; }

    [ValidateNever]
    public SectionSummaryViewModel ContributionsSummary { get; set; }

    [ValidateNever]
    public bool? CostsAndFunding { get; set; }

    public bool IsEditable { get; set; }

    public bool IsReadOnly => !IsEditable;

    public bool IsApplicationLocked { get; set; }

    public IsSectionCompleted IsSectionCompleted { get; set; }
}
