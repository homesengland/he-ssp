using HE.InvestmentLoans.Contract.Application.Queries;

namespace HE.InvestmentLoans.WWW.Models;

public class ApplicationDashboardModel
{
    public GetApplicationDashboardQueryResponse Data { get; set; }

    public bool IsOverviewSectionSelected { get; set; }
}
