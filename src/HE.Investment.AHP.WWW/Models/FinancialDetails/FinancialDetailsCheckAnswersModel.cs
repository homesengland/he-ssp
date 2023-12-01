using HE.Investments.Common.WWW.Components.SectionSummary;

namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsCheckAnswersModel : FinancialDetailsBaseModel
{
    public FinancialDetailsCheckAnswersModel()
    {
    }

    public FinancialDetailsCheckAnswersModel(
        Guid applicationId,
        string applicationName,
        IList<SectionSummaryItemModel>? landValueItems,
        IList<SectionSummaryItemModel>? costItems,
        IList<SectionSummaryItemModel>? contributionItems,
        bool? isCompleted)
        : base(applicationId, applicationName)
    {
        LandValueItems = landValueItems ?? new List<SectionSummaryItemModel>();
        CostItems = costItems ?? new List<SectionSummaryItemModel>();
        ContributionItems = contributionItems ?? new List<SectionSummaryItemModel>();
        IsCompleted = isCompleted;
    }

    public IList<SectionSummaryItemModel>? LandValueItems { get; set; }

    public IList<SectionSummaryItemModel>? CostItems { get; set; }

    public IList<SectionSummaryItemModel>? ContributionItems { get; set; }

    public bool? IsCompleted { get; set; }
}
