using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record UnCompleteSchemeCommand(string ApplicationId) : IRequest<OperationResult>, IUpdateSchemeCommand;
