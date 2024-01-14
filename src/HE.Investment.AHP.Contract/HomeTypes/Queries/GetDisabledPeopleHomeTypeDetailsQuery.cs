using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetDisabledPeopleHomeTypeDetailsQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId)
    : GetHomeTypeSegmentQueryBase<DisabledPeopleHomeTypeDetails>(ApplicationId, HomeTypeId);
