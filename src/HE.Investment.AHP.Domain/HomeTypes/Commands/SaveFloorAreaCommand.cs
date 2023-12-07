using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveFloorAreaCommand(string ApplicationId, string HomeTypeId, string? InternalFloorArea, YesNoType MeetNationallyDescribedSpaceStandards)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
