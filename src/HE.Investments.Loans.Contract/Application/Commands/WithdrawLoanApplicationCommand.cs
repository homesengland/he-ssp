using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Application.Commands;

public record WithdrawLoanApplicationCommand(LoanApplicationId LoanApplicationId, string WithdrawReason) : IRequest<OperationResult>;
