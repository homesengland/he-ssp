using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.User.Commands;

public record ProvideAcceptHeTermsAndConditionsCommand(string? HeTermsAndConditions) : IRequest<OperationResult>;
