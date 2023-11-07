using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeSalesRiskCommand(string SchemeId, string SalesRisk) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
