using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.SupportedHousingInformation)]
public class SupportedHousingInformationEntity : IHomeTypeSegmentEntity
{
    public SupportedHousingInformationEntity(
        YesNoType localCommissioningBodiesConsulted = YesNoType.Undefined,
        YesNoType shortStayAccommodation = YesNoType.Undefined,
        RevenueFundingType revenueFundingType = RevenueFundingType.Undefined)
    {
        LocalCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
        ShortStayAccommodation = shortStayAccommodation;
        RevenueFundingType = revenueFundingType;
    }

    public YesNoType LocalCommissioningBodiesConsulted { get; private set; }

    public YesNoType ShortStayAccommodation { get; private set; }

    public RevenueFundingType RevenueFundingType { get; private set; }

    public void ChangeLocalCommissioningBodiesConsulted(YesNoType localCommissioningBodiesConsulted)
    {
        LocalCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
    }

    public void ChangeShortStayAccommodation(YesNoType shortStayAccommodation)
    {
        ShortStayAccommodation = shortStayAccommodation;
    }

    public void ChangeRevenueFundingType(RevenueFundingType revenueFundingType)
    {
        RevenueFundingType = revenueFundingType;
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new SupportedHousingInformationEntity(LocalCommissioningBodiesConsulted, ShortStayAccommodation, RevenueFundingType);
    }

    public bool IsCompleted()
    {
        return LocalCommissioningBodiesConsulted.IsProvided()
               && ShortStayAccommodation.IsProvided()
               && RevenueFundingType != RevenueFundingType.Undefined;
    }
}
