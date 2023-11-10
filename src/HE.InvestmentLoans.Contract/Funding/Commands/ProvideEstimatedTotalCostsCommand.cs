using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Commands;

public record ProvideEstimatedTotalCostsCommand(LoanApplicationId LoanApplicationId, string? EstimatedTotalCosts) : IRequest<OperationResult>;
