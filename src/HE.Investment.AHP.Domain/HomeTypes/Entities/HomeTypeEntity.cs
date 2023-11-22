using System.Reflection;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity : IHomeTypeEntity
{
    private readonly IDictionary<HomeTypeSegmentType, IHomeTypeSegmentEntity> _segments;

    private bool _isModified;

    private HousingType _housingType;

    private HomeTypeName? _name;

    public HomeTypeEntity(
        ApplicationBasicInfo application,
        HomeTypeId? id = null,
        string? name = null,
        HousingType housingType = HousingType.Undefined,
        params IHomeTypeSegmentEntity[] segments)
    {
        Application = application;
        Id = id;
        _name = name.IsNotProvided() ? null : new HomeTypeName(name);
        _housingType = housingType;
        _segments = segments.ToDictionary(x => GetSegmentType(x.GetType()), x => x);
    }

    public ApplicationBasicInfo Application { get; }

    public HomeTypeId? Id { get; set; }

    public HomeTypeName? Name
    {
        get => _name;
        private set
        {
            if (_name != value)
            {
                _isModified = true;
            }

            _name = value;
        }
    }

    public HousingType HousingType
    {
        get => _housingType;
        private set
        {
            if (_housingType != value)
            {
                _isModified = true;
            }

            _housingType = value;
        }
    }

    public HomeInformationSegmentEntity HomeInformation => GetRequiredSegment<HomeInformationSegmentEntity>();

    public DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails => GetRequiredSegment<DisabledPeopleHomeTypeDetailsSegmentEntity>();

    public OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails => GetRequiredSegment<OlderPeopleHomeTypeDetailsSegmentEntity>();

    public DesignPlansSegmentEntity DesignPlans => GetRequiredSegment<DesignPlansSegmentEntity>();

    public SupportedHousingInformationEntity SupportedHousingInformation => GetRequiredSegment<SupportedHousingInformationEntity>();

    public bool IsNew => Id.IsNotProvided();

    public bool IsModified => _isModified || _segments.Any(x => x.Value.IsModified);

    // TODO: set this value when implementing Delivery Phases
    public bool IsUsedInDeliveryPhase => false;

    public void ChangeName(string? name)
    {
        Name = new HomeTypeName(name);
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

        HousingType = newHousingType;
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(Application, null, newName.Value, HousingType, _segments.Select(x => x.Value.Duplicate()).ToArray());
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
