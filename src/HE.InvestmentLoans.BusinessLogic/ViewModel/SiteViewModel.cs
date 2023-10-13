using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

public class SiteViewModel
{
    public SiteViewModel()
    {
        StateChanged = false;
    }

    public SectionStatus Status { get; set; }

    public string? DefaultName { get; set; }

    public string? Name { get; set; }

    public string? AffordableHomes { get; set; }

    public string? PlanningRef { get; set; }

    public string? PlanningRefEnter { get; set; }

    public string? Ownership { get; set; }

    public string? ManyHomes { get; set; }

    public string? GrantFunding { get; set; }

    public string[]? TypeHomes { get; set; }

    public string? TypeHomesOther { get; set; }

    public string? HomesToBuild { get; set; }

    public DateTime? PurchaseDate { get; set; }

    public string? Cost { get; set; }

    public string? Value { get; set; }

    public string? Source { get; set; }

    public string? LocationOption { get; set; }

    public string? LocationCoordinates { get; set; }

    public string? LocationLandRegistry { get; set; }

    public string? Location { get; set; }

    public string? PlanningStatus { get; set; }

    public string? GrantFundingName { get; set; }

    public string? GrantFundingSource { get; set; }

    public string? GrantFundingAmount { get; set; }

    public string? GrantFundingPurpose { get; set; }

    public string? Type { get; set; }

    public string? ChargesDebt { get; set; }

    public string? ChargesDebtInfo { get; set; }

    public Guid? Id { get; set; }

    public bool StateChanged { get; set; }

    public string? HasEstimatedStartDate { get; set; }

    public string? EstimatedStartDay { get; set; }

    public string? EstimatedStartMonth { get; set; }

    public string? EstimatedStartYear { get; set; }

    public string? DeleteProject { get; set; }

    public bool IsFlowCompleted { get; set; }

    public bool IsCompleted() => Status == SectionStatus.Completed;

    public bool IsInProgress() => Status == SectionStatus.InProgress;

    public SectionStatus GetSectionStatus()
    {
        return Status;
    }
}
