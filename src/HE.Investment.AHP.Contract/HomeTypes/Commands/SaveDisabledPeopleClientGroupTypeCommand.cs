using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SaveDisabledPeopleClientGroupTypeCommand(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId, DisabledPeopleClientGroupType ClientGroupType)
    : ISaveHomeTypeSegmentCommand;
