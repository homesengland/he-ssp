using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Security.Commands;

public record ConfirmSecuritySectionCommand(LoanApplicationId Id, string Answer) : IRequest<OperationResult>;
