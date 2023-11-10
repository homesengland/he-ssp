using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Funding.Commands;

public record ProvidePrivateSectorFundingCommand(LoanApplicationId LoanApplicationId, string? IsApplied, string? PrivateSectorFundingApplyResult, string? PrivateSectorFundingNotApplyingReason)
    : IRequest<OperationResult>;
