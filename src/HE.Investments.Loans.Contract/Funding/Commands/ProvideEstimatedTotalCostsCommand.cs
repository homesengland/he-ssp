using HE.Investments.Common.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Funding.Commands;

public record ProvideEstimatedTotalCostsCommand(LoanApplicationId LoanApplicationId, string? EstimatedTotalCosts) : IRequest<OperationResult>;
