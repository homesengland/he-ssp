using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Queries;
public record GetSecurity(LoanApplicationId Id) : IRequest<GetSecurityResponse>;

public record GetSecurityResponse(SecurityViewModel ViewModel);
