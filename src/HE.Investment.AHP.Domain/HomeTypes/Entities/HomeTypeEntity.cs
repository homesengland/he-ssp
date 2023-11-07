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

    public HomeInformationSegmentEntity HomeInformation => GetRequiredSegment<HomeInformationSegmentEntity>();

    public DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails => GetRequiredSegment<DisabledPeopleHomeTypeDetailsSegmentEntity>();

    public OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails => GetRequiredSegment<OlderPeopleHomeTypeDetailsSegmentEntity>();

    public DesignPlansSegmentEntity DesignPlans => GetRequiredSegment<DesignPlansSegmentEntity>();

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
            UpdateSegment(GetOptionalSegment<DesignPlansSegmentEntity>() ?? new DesignPlansSegmentEntity());
        }

        if (HousingType == HousingType.HomesForDisabledAndVulnerablePeople && newHousingType != HousingType.HomesForDisabledAndVulnerablePeople)
        {
            UpdateSegment(new DisabledPeopleHomeTypeDetailsSegmentEntity());
            UpdateSegment(GetOptionalSegment<DesignPlansSegmentEntity>() ?? new DesignPlansSegmentEntity());
        }

        HousingType = newHousingType;
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(newName.Value, HousingType, _segments.Select(x => x.Value.Duplicate()).ToArray());
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

    private TSegment GetRequiredSegment<TSegment>()
        where TSegment : IHomeTypeSegmentEntity
    {
        return GetOptionalSegment<TSegment>() ?? throw new InvalidOperationException($"Cannot get {typeof(TSegment).Name} segment because it does not exist.");
    }

    private TSegment? GetOptionalSegment<TSegment>()
        where TSegment : IHomeTypeSegmentEntity
    {
        return _segments.TryGetValue(GetSegmentType(typeof(TSegment)), out var segment) ? (TSegment)segment : default;
    }

    private void UpdateSegment<TSegment>(TSegment segment)
        where TSegment : IHomeTypeSegmentEntity
    {
        _segments[GetSegmentType(typeof(TSegment))] = segment;
    }
}
