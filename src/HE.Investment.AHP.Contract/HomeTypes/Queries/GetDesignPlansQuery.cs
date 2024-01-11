using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetDesignPlansQuery(AhpApplicationId ApplicationId, string HomeTypeId) : GetHomeTypeSegmentQueryBase<DesignPlans>(ApplicationId, HomeTypeId);
