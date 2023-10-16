using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Queries;

public record IsLoanApplicationExistQuery(LoanApplicationId LoanApplicationId) : IRequest<bool>;
