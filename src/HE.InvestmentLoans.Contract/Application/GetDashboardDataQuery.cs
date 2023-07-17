using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application;

public record GetDashboardDataQuery : IRequest<GetDashboardDataQueryResponse>;

public record GetDashboardDataQueryResponse(IList<UserLoanApplication> LoanApplications);
