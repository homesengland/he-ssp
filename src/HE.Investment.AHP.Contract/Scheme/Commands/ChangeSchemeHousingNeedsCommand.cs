using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public record ChangeSchemeHousingNeedsCommand(AhpApplicationId ApplicationId, string? MeetingLocalPriorities, string? MeetingLocalHousingNeed) : IRequest<OperationResult>, IUpdateSchemeCommand;
