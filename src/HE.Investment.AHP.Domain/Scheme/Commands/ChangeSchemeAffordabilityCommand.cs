using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeAffordabilityCommand(string SchemeId, string AffordabilityEvidence) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
