using System.Reflection;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.InvestmentLoans.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity : IHomeTypeEntity
{
    private readonly IDictionary<HomeTypeSegmentType, IHomeTypeSegmentEntity> _segments;

    public HomeTypeEntity(
        string? name = null,
        HousingType housingType = HousingType.Undefined,
        params IHomeTypeSegmentEntity[] segments)
    {
        ChangeName(name);
        ChangeHousingType(housingType);
        _segments = segments.ToDictionary(x => GetSegmentType(x.GetType()), x => x);
    }

    public HomeTypeId? Id { get; set; }

    public HomeTypeName? Name { get; private set; }

    public HousingType HousingType { get; private set; }

    public HomeInformationSegmentEntity HomeInformation => GetSegment<HomeInformationSegmentEntity>();

    public DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails => GetSegment<DisabledPeopleHomeTypeDetailsSegmentEntity>();

    public OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails => GetSegment<OlderPeopleHomeTypeDetailsSegmentEntity>();

    public bool IsNew => Id.IsNotProvided();

    public void ChangeName(string? name)
    {
        Name = name != null ? new HomeTypeName(name) : null;
    }

    public void ChangeHousingType(HousingType newHousingType)
    {
        if (HousingType == HousingType.HomesForOlderPeople && newHousingType != HousingType.HomesForOlderPeople)
        {
            UpdateSegment(new OlderPeopleHomeTypeDetailsSegmentEntity());
        }

        if (HousingType == HousingType.HomesForDisabledAndVulnerablePeople && newHousingType != HousingType.HomesForDisabledAndVulnerablePeople)
        {
            UpdateSegment(new DisabledPeopleHomeTypeDetailsSegmentEntity());
        }

        HousingType = newHousingType;
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(newName.Value, HousingType, _segments.Select(x => x.Value).ToArray());
    }

    public bool IsCompleted()
    {
        return Name.IsProvided() && _segments.All(x => x.Value.IsCompleted());
    }

    private static HomeTypeSegmentType GetSegmentType(Type segmentType)
    {
        var segmentTypeAttribute = segmentType.GetCustomAttributes<HomeTypeSegmentTypeAttribute>().FirstOrDefault();
        if (segmentTypeAttribute == null)
        {
            throw new ArgumentException($"{segmentType.Name} segment entity is missing {nameof(HomeTypeSegmentTypeAttribute)}.", nameof(segmentType));
        }

        return segmentTypeAttribute.SegmentType;
    }

    private TSegment GetSegment<TSegment>()
        where TSegment : IHomeTypeSegmentEntity
    {
        return (TSegment)_segments[GetSegmentType(typeof(TSegment))];
    }

    private void UpdateSegment<TSegment>(TSegment segment)
        where TSegment : IHomeTypeSegmentEntity
    {
        var segmentType = GetSegmentType(typeof(TSegment));
        _segments[segmentType] = segment;
    }
}
