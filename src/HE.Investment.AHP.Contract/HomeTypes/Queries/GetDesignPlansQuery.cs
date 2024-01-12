using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetDesignPlansQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : GetHomeTypeSegmentQueryBase<DesignPlans>(ApplicationId, HomeTypeId);
