using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.InvestmentLoans.Contract.User.Commands;

public record ProvideAcceptHeTermsAndConditionsCommand(string? HeTermsAndConditions) : IRequest<OperationResult>;
