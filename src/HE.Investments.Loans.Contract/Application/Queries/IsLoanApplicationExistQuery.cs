using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Queries;

public record IsLoanApplicationExistQuery(LoanApplicationId LoanApplicationId) : IRequest<bool>;
