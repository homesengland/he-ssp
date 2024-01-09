using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Scheme.Commands;

public record ChangeSchemeHousingNeedsCommand(string ApplicationId, string? MeetingLocalPriorities, string? MeetingLocalHousingNeed) : IRequest<OperationResult>, IUpdateSchemeCommand;
