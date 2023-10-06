using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Helper;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.WWW.Models;

public class ApplicationDashboardModel
{
    public LoanApplicationId LoanApplicationId { get; set; }

    public ApplicationStatus LoanApplicationStatus { get; set; }

    public GetApplicationDashboardQueryResponse Data { get; set; }

    public bool IsOverviewSectionSelected { get; set; }

    public bool CanBeWithdrawn()
    {
        var statusesAllowedForWithdraw = ApplicationStatusDivision.GetAllStatusesAllowedForWithdraw();
        return statusesAllowedForWithdraw.Contains(LoanApplicationStatus);
    }
}
