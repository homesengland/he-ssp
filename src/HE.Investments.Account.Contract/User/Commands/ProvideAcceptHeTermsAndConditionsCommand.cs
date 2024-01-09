using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.User.Commands;

public record ProvideAcceptHeTermsAndConditionsCommand(string? HeTermsAndConditions) : IRequest<OperationResult>;
