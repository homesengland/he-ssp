using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetHomeInformationQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId) : GetHomeTypeSegmentQueryBase<HomeInformation>(ApplicationId, HomeTypeId);
