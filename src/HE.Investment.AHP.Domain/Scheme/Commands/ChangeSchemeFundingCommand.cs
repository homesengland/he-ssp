using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeFundingCommand(string SchemeId, decimal? RequiredFunding, int? HousesToDeliver) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
