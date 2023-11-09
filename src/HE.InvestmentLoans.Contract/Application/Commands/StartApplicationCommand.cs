using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Application.Commands;

public record StartApplicationCommand(string ApplicationName) : IRequest<OperationResult<LoanApplicationId?>>;
