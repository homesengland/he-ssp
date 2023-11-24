using System.Reflection;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity : IHomeTypeEntity
{
    private readonly IDictionary<HomeTypeSegmentType, IHomeTypeSegmentEntity> _segments;

    private readonly ModificationTracker _modificationTracker = new();

    public HomeTypeEntity(
        ApplicationBasicInfo application,
        string? name,
        HousingType housingType,
        HomeTypeId? id = null,
        DateTime? createdOn = null,
        params IHomeTypeSegmentEntity[] segments)
    {
        Application = application;
        Name = new HomeTypeName(name);
        HousingType = housingType;
        Id = id ?? HomeTypeId.New();
        CreatedOn = createdOn;
        _segments = segments.ToDictionary(x => GetSegmentType(x.GetType()), x => x);
    }

    public ApplicationBasicInfo Application { get; }

    public HomeTypeId Id { get; set; }

    public HomeTypeName Name { get; private set; }

    public HousingType HousingType { get; private set; }

    public DateTime? CreatedOn { get; }

    public HomeInformationSegmentEntity HomeInformation => GetRequiredSegment<HomeInformationSegmentEntity>();

    public DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails => GetRequiredSegment<DisabledPeopleHomeTypeDetailsSegmentEntity>();

    public OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails => GetRequiredSegment<OlderPeopleHomeTypeDetailsSegmentEntity>();

    public DesignPlansSegmentEntity DesignPlans => GetRequiredSegment<DesignPlansSegmentEntity>();

    public SupportedHousingInformationEntity SupportedHousingInformation => GetRequiredSegment<SupportedHousingInformationEntity>();

    public bool IsNew => Id.IsNew;

    public bool IsModified => _modificationTracker.IsModified || _segments.Any(x => x.Value.IsModified);

    // TODO: set this value when implementing Delivery Phases
    public bool IsUsedInDeliveryPhase => false;

    public void ChangeName(string? name)
    {
        Name = _modificationTracker.Change(Name, new HomeTypeName(name));
    }

    public void ChangeHousingType(HousingType newHousingType)
    {
        if (HousingType == HousingType.HomesForOlderPeople && newHousingType != HousingType.HomesForOlderPeople)
        {
            UpdateSegment(new OlderPeopleHomeTypeDetailsSegmentEntity());
            UpdateSegment(GetOptionalSegment<DesignPlansSegmentEntity>() ?? new DesignPlansSegmentEntity(Application));
            UpdateSegment(GetOptionalSegment<SupportedHousingInformationEntity>() ?? new SupportedHousingInformationEntity());
        }

        if (HousingType == HousingType.HomesForDisabledAndVulnerablePeople && newHousingType != HousingType.HomesForDisabledAndVulnerablePeople)
        {
            UpdateSegment(new DisabledPeopleHomeTypeDetailsSegmentEntity());
            UpdateSegment(GetOptionalSegment<DesignPlansSegmentEntity>() ?? new DesignPlansSegmentEntity(Application));
            UpdateSegment(GetOptionalSegment<SupportedHousingInformationEntity>() ?? new SupportedHousingInformationEntity());
        }

        HousingType = _modificationTracker.Change(HousingType, newHousingType);
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(Application, newName.Value, HousingType, segments: _segments.Select(x => x.Value.Duplicate()).ToArray());
    }

    public bool IsCompleted()
    {
        return HousingType != HousingType.Undefined
               && Name.IsProvided()
               && _segments.Where(x => x.Value.IsRequired(HousingType)).All(x => x.Value.IsCompleted());
    }

    public bool HasSegment(HomeTypeSegmentType segmentType)
    {
        return _segments.ContainsKey(segmentType);
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
