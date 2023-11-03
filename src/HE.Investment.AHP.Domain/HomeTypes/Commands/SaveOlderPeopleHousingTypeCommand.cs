using HE.Investment.AHP.Contract.HomeTypes;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveOlderPeopleHousingTypeCommand(string ApplicationId, string HomeTypeId, OlderPeopleHousingType HousingType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
