using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Models.Summary;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;

public class FinancialDetailsCheckAnswersModel : FinancialDetailsBaseModel, IEditableViewModel
{
    public FinancialDetailsCheckAnswersModel()
    {
        AllowedOperations = new List<AhpApplicationOperation>();
    }

    public FinancialDetailsCheckAnswersModel(
        string applicationId,
        string applicationName,
        SectionSummaryViewModel landValueSummary,
        SectionSummaryViewModel costsSummary,
        SectionSummaryViewModel contributionsSummary,
        IsSectionCompleted isSectionCompleted,
        IReadOnlyCollection<AhpApplicationOperation> allowedOperations)
        : base(applicationId, applicationName)
    {
        LandValueSummary = landValueSummary;
        CostsSummary = costsSummary;
        ContributionsSummary = contributionsSummary;
        IsSectionCompleted = isSectionCompleted;
        AllowedOperations = allowedOperations;
    }

    [ValidateNever]
    public SectionSummaryViewModel LandValueSummary { get; set; }

    [ValidateNever]
    public SectionSummaryViewModel CostsSummary { get; set; }

    [ValidateNever]
    public SectionSummaryViewModel ContributionsSummary { get; set; }

    [ValidateNever]
    public bool? CostsAndFunding { get; set; }

    public IReadOnlyCollection<AhpApplicationOperation> AllowedOperations { get; set; }

    public bool IsEditable => AllowedOperations.Contains(AhpApplicationOperation.Modification);

    public bool IsReadOnly => !IsEditable;

    public IsSectionCompleted IsSectionCompleted { get; set; }
}
