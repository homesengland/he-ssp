using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveBuildingInformationCommand(string ApplicationId, string HomeTypeId, BuildingType BuildingType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
