using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeHousingNeedsCommand(string ApplicationId, string? TypeAndTenureJustification, string? SchemeAndProposalJustification) : IRequest<OperationResult>, IUpdateSchemeCommand;
