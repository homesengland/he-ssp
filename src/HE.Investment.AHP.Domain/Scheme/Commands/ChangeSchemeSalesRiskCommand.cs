using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeSalesRiskCommand(string SchemeId, string SalesRisk) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
