using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveFloorAreaStandardsCommand(AhpApplicationId ApplicationId, string HomeTypeId, IReadOnlyCollection<NationallyDescribedSpaceStandardType> NationallyDescribedSpaceStandards)
    : ISaveHomeTypeSegmentCommand;
