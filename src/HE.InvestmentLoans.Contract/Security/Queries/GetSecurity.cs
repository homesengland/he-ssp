using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Queries;
public record GetSecurity(LoanApplicationId Id, SecurityViewOption SecurityViewOption) : IRequest<GetSecurityResponse>;

public record GetSecurityResponse(SecurityViewModel ViewModel);
