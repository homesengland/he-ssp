using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Mappers;

public class TenureDetailsSegmentContractMapper : IHomeTypeSegmentContractMapper<TenureDetailsSegmentEntity, TenureDetails>
{
    public TenureDetails Map(ApplicationName applicationName, HomeTypeName homeTypeName, TenureDetailsSegmentEntity segment)
    {
        return new TenureDetails(
            applicationName.Name,
            homeTypeName.Value,
            segment.MarketValue?.Value,
            segment.MarketRentPerWeek?.Value,
            segment.RentPerWeek?.Value,
            segment.ProspectiveRentAsPercentageOfMarketRent?.Value,
            segment.TargetRentExceedMarketRent.IsNotProvided() ? YesNoType.Undefined : segment.TargetRentExceedMarketRent!.Value,
            segment.ExemptFromTheRightToSharedOwnership,
            segment.ExemptionJustification?.Value,
            segment.IsProspectiveRentIneligible,
            segment.InitialSale?.Value,
            segment.ExpectedFirstTranche?.Value,
            segment.RentAsPercentageOfTheUnsoldShare?.Value);
    }
}
