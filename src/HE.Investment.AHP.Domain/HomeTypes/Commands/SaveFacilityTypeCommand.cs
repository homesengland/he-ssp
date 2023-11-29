using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveFacilityTypeCommand(string ApplicationId, string HomeTypeId, FacilityType FacilityType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
