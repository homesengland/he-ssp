using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Commands;

public record WithdrawLoanApplicationCommand(LoanApplicationId LoanApplicationId, string WithdrawReason, string ApplicationStatus) : IRequest<OperationResult>;
