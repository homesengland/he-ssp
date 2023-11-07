using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeSalesRiskCommand(string ApplicationId, string SalesRisk) : IRequest<OperationResult>, IUpdateSchemeCommand;
