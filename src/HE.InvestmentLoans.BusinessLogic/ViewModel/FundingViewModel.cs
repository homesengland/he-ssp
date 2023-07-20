using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;

namespace HE.InvestmentLoans.BusinessLogic.ViewModel;

public class FundingViewModel
{
    public FundingViewModel()
    {
        State = FundingWorkflow.State.Index;
        StateChanged = false;
    }

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

    public FundingWorkflow.State State { get; set; }

    public bool StateChanged { get; set; }

    public void RemoveAlternativeRoutesData()
    {
        if (PrivateSectorFunding == "Yes")
        {
            PrivateSectorFundingReason = null;
        }
        else if (PrivateSectorFunding == "No")
        {
            PrivateSectorFundingResult = null;
        }

        if (AbnormalCosts == "No")
        {
            AbnormalCostsInfo = null;
        }

        if (Refinance == "repay")
        {
            RefinanceInfo = null;
        }
    }
}
