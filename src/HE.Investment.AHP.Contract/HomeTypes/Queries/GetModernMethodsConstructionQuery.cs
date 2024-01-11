using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetModernMethodsConstructionQuery(AhpApplicationId ApplicationId, string HomeTypeId)
    : GetHomeTypeSegmentQueryBase<ModernMethodsConstruction>(ApplicationId, HomeTypeId);
