using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.SupportedHousingInformation)]
public class SupportedHousingInformationEntity : IHomeTypeSegmentEntity
{
    private readonly List<RevenueFundingSourceType> _revenueFundingSources;

    public SupportedHousingInformationEntity(
        YesNoType localCommissioningBodiesConsulted = YesNoType.Undefined,
        YesNoType shortStayAccommodation = YesNoType.Undefined,
        RevenueFundingType revenueFundingType = RevenueFundingType.Undefined,
        IEnumerable<RevenueFundingSourceType>? revenueFundingSources = null)
    {
        LocalCommissioningBodiesConsulted = localCommissioningBodiesConsulted;
        ShortStayAccommodation = shortStayAccommodation;
        RevenueFundingType = revenueFundingType;
        _revenueFundingSources = revenueFundingSources?.ToList() ?? new List<RevenueFundingSourceType>();
    }

    public YesNoType LocalCommissioningBodiesConsulted { get; private set; }

    public YesNoType ShortStayAccommodation { get; private set; }

    public RevenueFundingType RevenueFundingType { get; private set; }

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

        _revenueFundingSources.Clear();
        _revenueFundingSources.AddRange(uniqueRevenueFundingSources);
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new SupportedHousingInformationEntity(LocalCommissioningBodiesConsulted, ShortStayAccommodation, RevenueFundingType, RevenueFundingSources);
    }

    public bool IsCompleted()
    {
        return LocalCommissioningBodiesConsulted != YesNoType.Undefined
               && ShortStayAccommodation != YesNoType.Undefined
               && RevenueFundingType != RevenueFundingType.Undefined
               && RevenueFundingSources.Any();
    }
}
