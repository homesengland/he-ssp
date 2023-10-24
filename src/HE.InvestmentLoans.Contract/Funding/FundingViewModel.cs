using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.ViewModels;

namespace HE.InvestmentLoans.Contract.Funding;

public class FundingViewModel : ICompletedSectionViewModel
{
    public FundingViewModel()
    {
        StateChanged = false;
    }

    public Guid LoanApplicationId { get; set; }

    public ApplicationStatus LoanApplicationStatus { get; set; }

    public string? GrossDevelopmentValue { get; set; }

    public string? TotalCosts { get; set; }

    public string? AbnormalCosts { get; set; }

    public string? AbnormalCostsInfo { get; set; }

    public string? PrivateSectorFunding { get; set; }

    public string? PrivateSectorFundingResult { get; set; }

    public string? PrivateSectorFundingReason { get; set; }

    public string? AdditionalProjects { get; set; }

    public string? Refinance { get; set; }

    public string? RefinanceInfo { get; set; }

    public string? CheckAnswers { get; set; }

    public SectionStatus State { get; set; }

    public bool StateChanged { get; set; }

    public bool IsFlowCompleted { get; set; }

    public void RemoveAlternativeRoutesData()
    {
        if (PrivateSectorFunding == CommonResponse.Yes)
        {
            PrivateSectorFundingReason = null;
        }
        else if (PrivateSectorFunding == CommonResponse.No)
        {
            PrivateSectorFundingResult = null;
        }

        if (AbnormalCosts == CommonResponse.No)
        {
            AbnormalCostsInfo = null;
        }

        if (Refinance == FundingFormOption.Repay)
        {
            RefinanceInfo = null;
        }
    }

    public void SetFlowCompletion(bool value)
    {
        IsFlowCompleted = value;
    }

    public void SetLoanApplicationId(Guid id)
    {
        LoanApplicationId = id;
    }

    public bool IsCompleted()
    {
        return State == SectionStatus.Completed;
    }

    public bool IsInProgress()
    {
        return State == SectionStatus.InProgress;
    }

    public bool IsReadOnly()
    {
        var readonlyStatuses = ApplicationStatusDivision.GetAllStatusesForReadonlyMode();
        return readonlyStatuses.Contains(LoanApplicationStatus);
    }

    public bool IsEditable() => IsReadOnly() is false;

    public ApplicationStatus GetApplicationStatus()
    {
        return LoanApplicationStatus;
    }
}
