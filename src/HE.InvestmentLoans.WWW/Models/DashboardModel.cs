using HE.InvestmentLoans.Common.Contract.Services.Interfaces;
using HE.InvestmentLoans.Contract.Application.Queries;

namespace HE.InvestmentLoans.WWW.Models;

public class DashboardModel
{
    public GetDashboardDataQueryResponse DashboardData { get; set; }

    public INotificationService NotificationService { get; set; }
}
