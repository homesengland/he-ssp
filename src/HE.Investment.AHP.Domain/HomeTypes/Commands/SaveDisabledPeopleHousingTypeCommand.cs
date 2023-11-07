using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveDisabledPeopleHousingTypeCommand(string ApplicationId, string HomeTypeId, DisabledPeopleHousingType HousingType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
