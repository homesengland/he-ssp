using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record UpdateSchemeTenureCommand(string Id, string Tenure) : IRequest<OperationResult<SchemeId>>, IUpdateSchemeCommand;
