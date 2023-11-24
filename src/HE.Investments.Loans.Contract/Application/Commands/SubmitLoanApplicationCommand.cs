using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Commands;

public record SubmitLoanApplicationCommand(LoanApplicationId LoanApplicationId) : IRequest;
