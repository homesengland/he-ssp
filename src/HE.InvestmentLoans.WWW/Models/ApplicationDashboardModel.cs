using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;

namespace HE.InvestmentLoans.WWW.Models;

public class ApplicationDashboardModel
{
    public LoanApplicationId LoanApplicationId { get; set; }

    public GetApplicationDashboardQueryResponse Data { get; set; }

    public bool IsOverviewSectionSelected { get; set; }
}
