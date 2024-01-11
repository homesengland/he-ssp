using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.HomeTypes.Attributes;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.Entities;

[HomeTypeSegmentType(HomeTypeSegmentType.HomeInformation)]
public class HomeInformationSegmentEntity : IHomeTypeSegmentEntity
{
    private readonly ModificationTracker _modificationTracker;

    private readonly List<NationallyDescribedSpaceStandardType> _nationallyDescribedSpaceStandards;

    public HomeInformationSegmentEntity(
        NumberOfHomes? numberOfHomes = null,
        NumberOfBedrooms? numberOfBedrooms = null,
        MaximumOccupancy? maximumOccupancy = null,
        NumberOfStoreys? numberOfStoreys = null,
        YesNoType intendedAsMoveOnAccommodation = YesNoType.Undefined,
        PeopleGroupForSpecificDesignFeaturesType peopleGroupForSpecificDesignFeatures = PeopleGroupForSpecificDesignFeaturesType.Undefined,
        BuildingType buildingType = BuildingType.Undefined,
        YesNoType customBuild = YesNoType.Undefined,
        FacilityType facilityType = FacilityType.Undefined,
        YesNoType accessibilityStandards = YesNoType.Undefined,
        AccessibilityCategoryType accessibilityCategory = AccessibilityCategoryType.Undefined,
        FloorArea? internalFloorArea = null,
        YesNoType meetNationallyDescribedSpaceStandards = YesNoType.Undefined,
        IEnumerable<NationallyDescribedSpaceStandardType>? nationallyDescribedSpaceStandards = null)
    {
        _modificationTracker = new ModificationTracker(() => SegmentModified?.Invoke());
        NumberOfHomes = numberOfHomes;
        NumberOfBedrooms = numberOfBedrooms;
        MaximumOccupancy = maximumOccupancy;
        NumberOfStoreys = numberOfStoreys;
        IntendedAsMoveOnAccommodation = intendedAsMoveOnAccommodation;
        PeopleGroupForSpecificDesignFeatures = peopleGroupForSpecificDesignFeatures;
        BuildingType = buildingType;
        CustomBuild = customBuild;
        FacilityType = facilityType;
        AccessibilityStandards = accessibilityStandards;
        AccessibilityCategory = accessibilityCategory;
        InternalFloorArea = internalFloorArea;
        MeetNationallyDescribedSpaceStandards = meetNationallyDescribedSpaceStandards;
        _nationallyDescribedSpaceStandards = nationallyDescribedSpaceStandards?.ToList() ?? new List<NationallyDescribedSpaceStandardType>();
    }

    public event EntityModifiedEventHandler SegmentModified;

    public bool IsModified => _modificationTracker.IsModified;

    public NumberOfHomes? NumberOfHomes { get; private set; }

    public NumberOfBedrooms? NumberOfBedrooms { get; private set; }

    public MaximumOccupancy? MaximumOccupancy { get; private set; }

    public NumberOfStoreys? NumberOfStoreys { get; private set; }

    public YesNoType IntendedAsMoveOnAccommodation { get; private set; }

    public PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures { get; private set; }

    public BuildingType BuildingType { get; private set; }

    public YesNoType CustomBuild { get; private set; }

    public FacilityType FacilityType { get; private set; }

    public YesNoType AccessibilityStandards { get; private set; }

    public AccessibilityCategoryType AccessibilityCategory { get; private set; }

    public FloorArea? InternalFloorArea { get; private set; }

    public YesNoType MeetNationallyDescribedSpaceStandards { get; private set; }

    public IReadOnlyCollection<NationallyDescribedSpaceStandardType> NationallyDescribedSpaceStandards => _nationallyDescribedSpaceStandards;

    public void ChangeNumberOfHomes(string? numberOfHomes)
    {
        var newValue = numberOfHomes.IsProvided() ? new NumberOfHomes(numberOfHomes) : null;
        NumberOfHomes = _modificationTracker.Change(NumberOfHomes, newValue);
    }

    public void ChangeNumberOfBedrooms(string? numberOfBedrooms)
    {
        var newValue = numberOfBedrooms.IsProvided() ? new NumberOfBedrooms(numberOfBedrooms) : null;
        NumberOfBedrooms = _modificationTracker.Change(NumberOfBedrooms, newValue);
    }

    public void ChangeMaximumOccupancy(string? maximumOccupancy)
    {
        var newValue = maximumOccupancy.IsProvided() ? new MaximumOccupancy(maximumOccupancy) : null;
        MaximumOccupancy = _modificationTracker.Change(MaximumOccupancy, newValue);
    }

    public void ChangeNumberOfStoreys(string? numberOfStoreys)
    {
        var newValue = numberOfStoreys.IsProvided() ? new NumberOfStoreys(numberOfStoreys) : null;
        NumberOfStoreys = _modificationTracker.Change(NumberOfStoreys, newValue);
    }

    public void ChangeIntendedAsMoveOnAccommodation(YesNoType intendedAsMoveOnAccommodation)
    {
        IntendedAsMoveOnAccommodation = _modificationTracker.Change(IntendedAsMoveOnAccommodation, intendedAsMoveOnAccommodation);
    }

    public void ChangePeopleGroupForSpecificDesignFeatures(PeopleGroupForSpecificDesignFeaturesType peopleGroupForSpecificDesignFeatures)
    {
        PeopleGroupForSpecificDesignFeatures = _modificationTracker.Change(PeopleGroupForSpecificDesignFeatures, peopleGroupForSpecificDesignFeatures);
    }

    public void ChangeBuildingType(BuildingType buildingType)
    {
        BuildingType = _modificationTracker.Change(BuildingType, buildingType);
    }

    public void ChangeCustomBuild(YesNoType customBuild)
    {
        CustomBuild = _modificationTracker.Change(CustomBuild, customBuild);
    }

    public void ChangeFacilityType(FacilityType facilityType)
    {
        FacilityType = _modificationTracker.Change(FacilityType, facilityType);
    }

    public void ChangeAccessibilityStandards(YesNoType accessibilityStandards)
    {
        AccessibilityStandards = _modificationTracker.Change(AccessibilityStandards, accessibilityStandards);
        if (AccessibilityStandards != YesNoType.Yes)
        {
            AccessibilityCategory = AccessibilityCategoryType.Undefined;
        }
    }

    public void ChangeAccessibilityCategory(AccessibilityCategoryType accessibilityCategory)
    {
        AccessibilityCategory = _modificationTracker.Change(AccessibilityCategory, accessibilityCategory);
    }

    public void ChangeInternalFloorArea(string? floorArea)
    {
        var newValue = floorArea.IsProvided() ? new FloorArea(floorArea) : null;
        InternalFloorArea = _modificationTracker.Change(InternalFloorArea, newValue);
    }

    public void ChangeMeetNationallyDescribedSpaceStandards(YesNoType meetNationallyDescribedSpaceStandards)
    {
        MeetNationallyDescribedSpaceStandards = _modificationTracker.Change(MeetNationallyDescribedSpaceStandards, meetNationallyDescribedSpaceStandards);
    }

    public void ChangeNationallyDescribedSpaceStandards(IEnumerable<NationallyDescribedSpaceStandardType> nationallyDescribedSpaceStandards)
    {
        var uniqueNationallyDescribedSpaceStandards = nationallyDescribedSpaceStandards.Distinct().OrderBy(x => x).ToList();
        if (uniqueNationallyDescribedSpaceStandards.Contains(NationallyDescribedSpaceStandardType.NoneOfThese) && uniqueNationallyDescribedSpaceStandards.Count > 1)
        {
            OperationResult.New()
                .AddValidationError(
                    nameof(NationallyDescribedSpaceStandards),
                    ValidationErrorMessage.ExclusiveOptionSelected("Nationally Described Space Standards", NationallyDescribedSpaceStandardType.NoneOfThese.GetDescription()))
                .CheckErrors();
        }

        if (!_nationallyDescribedSpaceStandards.SequenceEqual(uniqueNationallyDescribedSpaceStandards))
        {
            _nationallyDescribedSpaceStandards.Clear();
            _nationallyDescribedSpaceStandards.AddRange(uniqueNationallyDescribedSpaceStandards);
            _modificationTracker.MarkAsModified();
        }
    }

    public IHomeTypeSegmentEntity Duplicate()
    {
        return new HomeInformationSegmentEntity(
            NumberOfHomes,
            NumberOfBedrooms,
            MaximumOccupancy,
            NumberOfStoreys,
            IntendedAsMoveOnAccommodation,
            PeopleGroupForSpecificDesignFeatures,
            BuildingType,
            CustomBuild,
            FacilityType,
            AccessibilityStandards,
            AccessibilityCategory,
            InternalFloorArea,
            MeetNationallyDescribedSpaceStandards,
            NationallyDescribedSpaceStandards);
    }

    public bool IsRequired(HousingType housingType)
    {
        return true;
    }

    public bool IsCompleted(HousingType housingType, Tenure tenure)
    {
        return NumberOfHomes.IsProvided()
               && NumberOfBedrooms.IsProvided()
               && MaximumOccupancy.IsProvided()
               && NumberOfStoreys.IsProvided()
               && BuildingType != BuildingType.Undefined
               && CustomBuild != YesNoType.Undefined
               && FacilityType != FacilityType.Undefined
               && AccessibilityStandards != YesNoType.Undefined
               && InternalFloorArea.IsProvided()
               && MeetNationallyDescribedSpaceStandards != YesNoType.Undefined
               && BuildConditionalRouteCompletionPredicates(housingType).All(isCompleted => isCompleted());
    }

    public void HousingTypeChanged(HousingType sourceHousingType, HousingType targetHousingType)
    {
        if (targetHousingType is HousingType.HomesForOlderPeople or HousingType.HomesForDisabledAndVulnerablePeople)
        {
            ChangeIntendedAsMoveOnAccommodation(YesNoType.Undefined);
        }

        if (targetHousingType is HousingType.Undefined or HousingType.General)
        {
            ChangePeopleGroupForSpecificDesignFeatures(PeopleGroupForSpecificDesignFeaturesType.Undefined);
        }

        if (targetHousingType is HousingType.General && BuildingType is BuildingType.Bedsit)
        {
            ChangeBuildingType(BuildingType.Undefined);
        }
    }

    private IEnumerable<Func<bool>> BuildConditionalRouteCompletionPredicates(HousingType housingType)
    {
        if (housingType is HousingType.Undefined or HousingType.General)
        {
            yield return () => IntendedAsMoveOnAccommodation != YesNoType.Undefined;
        }

        if (housingType is HousingType.HomesForOlderPeople or HousingType.HomesForDisabledAndVulnerablePeople)
        {
            yield return () => PeopleGroupForSpecificDesignFeatures != PeopleGroupForSpecificDesignFeaturesType.Undefined;
        }

        if (AccessibilityStandards == YesNoType.Yes)
        {
            yield return () => AccessibilityCategory != AccessibilityCategoryType.Undefined;
        }

        if (MeetNationallyDescribedSpaceStandards == YesNoType.No)
        {
            yield return () => NationallyDescribedSpaceStandards.Any();
        }
    }
}
