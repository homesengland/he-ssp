using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SaveDisabledPeopleClientGroupTypeCommand(string ApplicationId, string HomeTypeId, DisabledPeopleClientGroupType ClientGroupType)
    : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
