using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.SupportedHousingInformation)]
public class SupportedHousingInformationEntity : IHomeTypeSegmentEntity
{
    private readonly List<RevenueFundingSourceType> _revenueFundingSources;

    private YesNoType _localCommissioningBodiesConsulted;

    private YesNoType _shortStayAccommodation;

    private RevenueFundingType _revenueFundingType;

    public SupportedHousingInformationEntity(
        YesNoType localCommissioningBodiesConsulted = YesNoType.Undefined,
        YesNoType shortStayAccommodation = YesNoType.Undefined,
        RevenueFundingType revenueFundingType = RevenueFundingType.Undefined,
        IEnumerable<RevenueFundingSourceType>? revenueFundingSources = null)
    {
        _localCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
        _shortStayAccommodation = shortStayAccommodation;
        _revenueFundingType = revenueFundingType;
        LocalCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
        ShortStayAccommodation = shortStayAccommodation;
        RevenueFundingType = revenueFundingType;
        _revenueFundingSources = revenueFundingSources?.ToList() ?? new List<RevenueFundingSourceType>();
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

    public IReadOnlyCollection<RevenueFundingSourceType> RevenueFundingSources => _revenueFundingSources;

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

    public void ChangeSources(IEnumerable<RevenueFundingSourceType> revenueFundingSources)
    {
        var uniqueRevenueFundingSources = revenueFundingSources.Distinct().ToList();

        if (!_revenueFundingSources.SequenceEqual(uniqueRevenueFundingSources))
        {
            _revenueFundingSources.Clear();
            _revenueFundingSources.AddRange(uniqueRevenueFundingSources);
            IsModified = true;
        }
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new SupportedHousingInformationEntity(LocalCommissioningBodiesConsulted, ShortStayAccommodation, RevenueFundingType, RevenueFundingSources);
    }

    public bool IsRequired(HousingType housingType)
    {
        return housingType is HousingType.HomesForOlderPeople or HousingType.HomesForDisabledAndVulnerablePeople;
    }

    public bool IsCompleted()
    {
        return LocalCommissioningBodiesConsulted != YesNoType.Undefined
               && ShortStayAccommodation != YesNoType.Undefined
               && RevenueFundingType != RevenueFundingType.Undefined
               && RevenueFundingSources.Any();
    }
}
