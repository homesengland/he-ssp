using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveFloorAreaCommand(AhpApplicationId ApplicationId, string HomeTypeId, string? FloorArea, YesNoType MeetNationallyDescribedSpaceStandards)
    : ISaveHomeTypeSegmentCommand;
