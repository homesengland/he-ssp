using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetOlderPeopleHomeTypeDetailsQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId)
    : GetHomeTypeSegmentQueryBase<OlderPeopleHomeTypeDetails>(ApplicationId, HomeTypeId);
