using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Commands;

public record ProvideAbnormalCostsCommand(LoanApplicationId LoanApplicationId, string? IsAnyAbnormalCost, string? AbnormalCostsAdditionalInformation) : IRequest<OperationResult>;
