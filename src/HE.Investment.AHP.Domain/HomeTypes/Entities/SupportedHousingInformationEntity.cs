using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.SupportedHousingInformation)]
public class SupportedHousingInformationEntity : IHomeTypeSegmentEntity
{
    private YesNoType _localCommissioningBodiesConsulted;

    private YesNoType _shortStayAccommodation;

    private RevenueFundingType _revenueFundingType;

    public SupportedHousingInformationEntity(
        YesNoType localCommissioningBodiesConsulted = YesNoType.Undefined,
        YesNoType shortStayAccommodation = YesNoType.Undefined,
        RevenueFundingType revenueFundingType = RevenueFundingType.Undefined)
    {
        _localCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
        _shortStayAccommodation = shortStayAccommodation;
        _revenueFundingType = revenueFundingType;
    }

    public bool IsModified { get; private set; }

    public YesNoType LocalCommissioningBodiesConsulted
    {
        get => _localCommissioningBodiesConsulted;
        private set
        {
            if (_localCommissioningBodiesConsulted != value)
            {
                IsModified = true;
            }

            _localCommissioningBodiesConsulted = value;
        }
    }

    public YesNoType ShortStayAccommodation
    {
        get => _shortStayAccommodation;
        private set
        {
            if (_shortStayAccommodation != value)
            {
                IsModified = true;
            }

            _shortStayAccommodation = value;
        }
    }

    public RevenueFundingType RevenueFundingType
    {
        get => _revenueFundingType;
        private set
        {
            if (_revenueFundingType != value)
            {
                IsModified = true;
            }

            _revenueFundingType = value;
        }
    }

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

    public bool IsRequired(HousingType housingType)
    {
        return housingType is HousingType.HomesForOlderPeople or HousingType.HomesForDisabledAndVulnerablePeople;
    }

    public bool IsCompleted()
    {
        return LocalCommissioningBodiesConsulted != YesNoType.Undefined
               && ShortStayAccommodation != YesNoType.Undefined
               && RevenueFundingType != RevenueFundingType.Undefined;
    }
}
