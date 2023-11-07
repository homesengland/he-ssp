using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeHousingNeedsCommand(string SchemeId, string TypeAndTenureJustification, string SchemeAndProposalJustification) : IRequest<OperationResult<SchemeId?>>, IUpdateSchemeCommand;
