using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetModernMethodsConstructionQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId)
    : GetHomeTypeSegmentQueryBase<ModernMethodsConstruction>(ApplicationId, HomeTypeId);
