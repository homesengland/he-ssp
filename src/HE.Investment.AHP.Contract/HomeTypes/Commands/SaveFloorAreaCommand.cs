using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveFloorAreaCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, string? FloorArea, YesNoType MeetNationallyDescribedSpaceStandards)
    : ISaveHomeTypeSegmentCommand;
