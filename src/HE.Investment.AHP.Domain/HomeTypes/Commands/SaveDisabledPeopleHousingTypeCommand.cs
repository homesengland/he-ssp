using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveDisabledPeopleHousingTypeCommand(string ApplicationId, string HomeTypeId, DisabledPeopleHousingType HousingType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
