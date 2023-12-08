using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class TenureDetailsSegmentContractMapper : IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails>
{
    public TenureDetails Map(ApplicationName applicationName, HomeTypeName homeTypeName, TenureDetailsSegmentEntity segment)
    {
        return new TenureDetails(
            applicationName.Name,
            homeTypeName.Value,
            segment.HomeMarketValue?.Value,
            segment.HomeWeeklyRent?.Value,
            segment.AffordableWeeklyRent?.Value,
            segment.AffordableRentAsPercentageOfMarketRent?.Value,
            segment.TargetRentExceedMarketRent.IsNotProvided() ? YesNoType.Undefined : segment.TargetRentExceedMarketRent!.Value,
            segment.ExemptFromTheRightToSharedOwnership,
            segment.ExemptionJustification?.Value,
            segment.IsExceeding80PercentOfMarketRent);
    }
}
