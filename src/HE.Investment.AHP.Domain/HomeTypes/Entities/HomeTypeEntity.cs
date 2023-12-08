using System.Reflection;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity : IHomeTypeEntity
{
    private readonly IDictionary<HomeTypeSegmentType, IHomeTypeSegmentEntity> _segments;

    private readonly ModificationTracker _modificationTracker = new();

    public HomeTypeEntity(
        ApplicationBasicInfo application,
        string? name,
        HousingType housingType,
        SectionStatus status,
        HomeTypeId? id = null,
        DateTime? createdOn = null,
        params IHomeTypeSegmentEntity[] segments)
    {
        Application = application;
        Name = new HomeTypeName(name);
        HousingType = housingType;
        Status = status;
        Id = id ?? HomeTypeId.New();
        CreatedOn = createdOn;
        _segments = segments.ToDictionary(x => GetSegmentType(x.GetType()), x => x);

        foreach (var segment in segments)
        {
            segment.SegmentModified += MarkAsNotCompleted;
        }
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

    public SupportedHousingInformationSegmentEntity SupportedHousingInformation => GetRequiredSegment<SupportedHousingInformationSegmentEntity>();

    public TenureDetailsEntity TenureDetails => GetRequiredSegment<TenureDetailsEntity>();

    public bool IsNew => Id.IsNew;

    public bool IsModified => _modificationTracker.IsModified || _segments.Any(x => x.Value.IsModified);

    // TODO: set this value when implementing Delivery Phases
    public bool IsUsedInDeliveryPhase => false;

    public SectionStatus Status { get; private set; }

    public void CompleteHomeType(IsSectionCompleted isSectionCompleted)
    {
        if (isSectionCompleted == IsSectionCompleted.Undefied)
        {
            OperationResult.New().AddValidationError(nameof(IsSectionCompleted), "Select whether you have completed this section").CheckErrors();
        }

        if (isSectionCompleted == IsSectionCompleted.No)
        {
            Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
            return;
        }

        var canBeCompleted = HousingType != HousingType.Undefined
                             && Name.IsProvided()
                             && _segments.Where(x => x.Value.IsRequired(HousingType)).All(x => x.Value.IsCompleted());
        if (!canBeCompleted)
        {
            OperationResult.New()
                .AddValidationError(nameof(IsSectionCompleted), "You have not completed this section. Select no if you want to come back later")
                .CheckErrors();
        }

        Status = _modificationTracker.Change(Status, SectionStatus.Completed);
    }

    public void ChangeName(string? name)
    {
        Name = _modificationTracker.Change(Name, new HomeTypeName(name));
        MarkAsNotCompleted();
    }

    public void ChangeHousingType(HousingType newHousingType)
    {
        if (HousingType == newHousingType)
        {
            return;
        }

        foreach (var (_, segment) in _segments)
        {
            segment.HousingTypeChanged(HousingType, newHousingType);
        }

        HousingType = _modificationTracker.Change(HousingType, newHousingType);
        MarkAsNotCompleted();
    }

    public HomeTypeEntity Duplicate(HomeTypeName newName)
    {
        return new HomeTypeEntity(
            Application,
            newName.Value,
            HousingType,
            SectionStatus.InProgress,
            segments: _segments.Select(x => x.Value.Duplicate()).ToArray());
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
        return _segments.TryGetValue(GetSegmentType(typeof(TSegment)), out var segment)
            ? (TSegment)segment
            : throw new InvalidOperationException($"Cannot get {typeof(TSegment).Name} segment because it does not exist.");
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }
}
