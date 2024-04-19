using System.Collections.ObjectModel;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Infrastructure.Events;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

public class HomeTypeEntity : DomainEntity, IHomeTypeEntity
{
    private readonly SiteBasicInfo _site;

    private readonly ModificationTracker _modificationTracker = new();

    public HomeTypeEntity(
        ApplicationBasicInfo application,
        SiteBasicInfo site,
        string? name,
        HousingType housingType,
        SectionStatus status,
        HomeTypeId? id = null,
        DateTime? createdOn = null,
        HomeInformationSegmentEntity? homeInformation = null,
        DisabledPeopleHomeTypeDetailsSegmentEntity? disabledPeopleHomeTypeDetails = null,
        OlderPeopleHomeTypeDetailsSegmentEntity? olderPeopleHomeTypeDetails = null,
        DesignPlansSegmentEntity? designPlans = null,
        SupportedHousingInformationSegmentEntity? supportedHousingInformation = null,
        TenureDetailsSegmentEntity? tenureDetails = null,
        ModernMethodsConstructionSegmentEntity? modernMethodsConstruction = null)
    {
        _site = site;
        Application = application;
        Name = new HomeTypeName(name);
        HousingType = housingType;
        Status = status;
        Id = id ?? HomeTypeId.New();
        CreatedOn = createdOn;
        HomeInformation = homeInformation ?? new HomeInformationSegmentEntity(application);
        DisabledPeopleHomeTypeDetails = disabledPeopleHomeTypeDetails ?? new DisabledPeopleHomeTypeDetailsSegmentEntity();
        OlderPeopleHomeTypeDetails = olderPeopleHomeTypeDetails ?? new OlderPeopleHomeTypeDetailsSegmentEntity();
        DesignPlans = designPlans ?? new DesignPlansSegmentEntity(application);
        SupportedHousingInformation = supportedHousingInformation ?? new SupportedHousingInformationSegmentEntity();
        TenureDetails = tenureDetails ?? new TenureDetailsSegmentEntity();
        ModernMethodsConstruction = modernMethodsConstruction ?? new ModernMethodsConstructionSegmentEntity(site.SiteUsingModernMethodsOfConstruction);

        foreach (var segment in Segments)
        {
            segment.SegmentModified += MarkAsNotCompleted;
        }
    }

    public ApplicationBasicInfo Application { get; }

    public HomeTypeId Id { get; set; }

    public HomeTypeName Name { get; private set; }

    public HousingType HousingType { get; private set; }

    public DateTime? CreatedOn { get; }

    public HomeInformationSegmentEntity HomeInformation { get; }

    public DisabledPeopleHomeTypeDetailsSegmentEntity DisabledPeopleHomeTypeDetails { get; }

    public OlderPeopleHomeTypeDetailsSegmentEntity OlderPeopleHomeTypeDetails { get; }

    public DesignPlansSegmentEntity DesignPlans { get; }

    public SupportedHousingInformationSegmentEntity SupportedHousingInformation { get; }

    public TenureDetailsSegmentEntity TenureDetails { get; }

    public ModernMethodsConstructionSegmentEntity ModernMethodsConstruction { get; }

    public bool IsNew => Id.IsNew;

    public bool IsModified => _modificationTracker.IsModified || Segments.Any(x => x.IsModified);

    public SectionStatus Status { get; private set; }

    private IEnumerable<IHomeTypeSegmentEntity> Segments => new IHomeTypeSegmentEntity[]
    {
        HomeInformation,
        DisabledPeopleHomeTypeDetails,
        OlderPeopleHomeTypeDetails,
        DesignPlans,
        SupportedHousingInformation,
        TenureDetails,
        ModernMethodsConstruction,
    };

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
                             && Segments.Where(x => x.IsRequired(HousingType)).All(x => x.IsCompleted(HousingType, Application.Tenure));
        if (!canBeCompleted)
        {
            OperationResult.New()
                .AddValidationError(nameof(IsSectionCompleted), ValidationErrorMessage.SectionIsNotCompleted)
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

        foreach (var segment in Segments)
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
            _site,
            newName.Value,
            HousingType,
            SectionStatus.InProgress,
            null,
            null,
            HomeInformation.Duplicate(),
            DisabledPeopleHomeTypeDetails.Duplicate(),
            OlderPeopleHomeTypeDetails.Duplicate(),
            DesignPlans.Duplicate(),
            SupportedHousingInformation.Duplicate(),
            TenureDetails.Duplicate(),
            ModernMethodsConstruction.Duplicate());
    }

    public override IReadOnlyList<IDomainEvent> GetDomainEventsAndRemove()
    {
        return new ReadOnlyCollection<IDomainEvent>(base.GetDomainEventsAndRemove()
            .Concat(Segments.SelectMany(x => x.GetDomainEventsAndRemove()))
            .ToList());
    }

    private void MarkAsNotCompleted()
    {
        Status = _modificationTracker.Change(Status, SectionStatus.InProgress);
    }
}
