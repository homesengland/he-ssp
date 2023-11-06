using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeAffordabilityCommand(string SchemeId, string AffordabilityEvidence) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
