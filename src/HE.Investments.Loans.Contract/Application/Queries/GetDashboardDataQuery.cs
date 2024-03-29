using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Queries;

public class GetDashboardDataQuery : IRequest<GetDashboardDataQueryResponse>
{
}

public record GetDashboardDataQueryResponse(IList<UserLoanApplication> LoanApplications, string? AccountName);
