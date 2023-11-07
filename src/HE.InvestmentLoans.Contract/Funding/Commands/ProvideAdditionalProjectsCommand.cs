using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Commands;

public record ProvideAdditionalProjectsCommand(LoanApplicationId LoanApplicationId, string? IsThereAnyAdditionalProject) : IRequest<OperationResult>;
