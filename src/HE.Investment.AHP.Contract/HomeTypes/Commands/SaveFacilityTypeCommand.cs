using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveFacilityTypeCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, FacilityType FacilityType)
    : ISaveHomeTypeSegmentCommand;
