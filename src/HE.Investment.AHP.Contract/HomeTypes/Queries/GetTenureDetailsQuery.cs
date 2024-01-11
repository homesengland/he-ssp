using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetTenureDetailsQuery(AhpApplicationId ApplicationId, string HomeTypeId)
    : GetHomeTypeSegmentQueryBase<TenureDetails>(ApplicationId, HomeTypeId);
