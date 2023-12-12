using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeHousingNeedsCommand(string ApplicationId, string? MeetingLocalPriorities, string? MeetingLocalHousingNeed) : IRequest<OperationResult>, IUpdateSchemeCommand;
