using HE.Investments.Common.Contract.Validators;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.Investments.Loans.Contract.Funding.Commands;

public record ProvidePrivateSectorFundingCommand(LoanApplicationId LoanApplicationId, string? IsApplied, string? PrivateSectorFundingApplyResult, string? PrivateSectorFundingNotApplyingReason)
    : IRequest<OperationResult>;
