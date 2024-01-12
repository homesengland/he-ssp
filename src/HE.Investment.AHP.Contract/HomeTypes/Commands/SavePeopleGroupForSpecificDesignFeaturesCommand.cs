using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Contract.HomeTypes.Commands;

public record SavePeopleGroupForSpecificDesignFeaturesCommand(
    AhpApplicationId ApplicationId,
    HomeTypeId HomeTypeId,
    PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures) : ISaveHomeTypeSegmentCommand;
