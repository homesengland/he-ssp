using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeFundingCommand(string SchemeId, decimal? RequiredFunding, int? HousesToDeliver) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
