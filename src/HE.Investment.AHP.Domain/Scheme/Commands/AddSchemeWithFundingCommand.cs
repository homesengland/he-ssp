using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record AddSchemeWithFundingCommand(string ApplicationId, decimal? RequiredFunding, int? HousesToDeliver) : IRequest<OperationResult<SchemeId?>>;
