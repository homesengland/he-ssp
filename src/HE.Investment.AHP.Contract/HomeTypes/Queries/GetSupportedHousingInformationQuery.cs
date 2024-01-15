using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes.Queries;

public record GetSupportedHousingInformationQuery(AhpApplicationId ApplicationId, HomeTypeId HomeTypeId)
    : GetHomeTypeSegmentQueryBase<SupportedHousingInformation>(ApplicationId, HomeTypeId);
