using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public record SavePeopleGroupForSpecificDesignFeaturesCommand(
    string ApplicationId,
    string HomeTypeId,
    PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures) : SaveHomeTypeSegmentCommandBase(ApplicationId, HomeTypeId);
