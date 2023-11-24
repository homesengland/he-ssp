using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Application.Helper;
using HE.Investments.Loans.Contract.Application.Queries;
using HE.Investments.Loans.Contract.Application.ValueObjects;

namespace HE.Investments.Loans.WWW.Models;

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
