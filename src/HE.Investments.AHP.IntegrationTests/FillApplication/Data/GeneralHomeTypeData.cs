using System.Globalization;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data;

public class GeneralHomeTypeData : INestedItemData
{
    public string Id { get; private set; }

    public string Name { get; private set; }

    public HousingType HousingType => HousingType.General;

    public int NumberOfHomes { get; private set; }

    public int NumberOfBedrooms { get; private set; }

    public int MaximumOccupancy { get; private set; }

    public int NumberOfStoreys { get; private set; }

    public YesNoType MoveOnAccommodation { get; private set; }

    public BuildingType BuildingType { get; private set; }

    public YesNoType CustomBuild { get; private set; }

    public FacilityType FacilityType { get; private set; }

    public YesNoType AccessibilityStandards { get; private set; }

    public AccessibilityCategoryType AccessibilityCategory { get; private set; }

    public decimal FloorArea { get; private set; }

    public YesNoType MeetSpaceStandards { get; private set; }

    public NationallyDescribedSpaceStandardType SpaceStandards { get; private set; }

    public int MarketValue { get; private set; }

    public decimal MarketRent { get; private set; }

    public decimal ProspectiveRent { get; private set; }

    public string ProspectiveRentPercentage { get; private set; }

    public YesNoType Exceeds80PercentOfMarketRent { get; private set; }

    public YesNoType ExemptFromTheRightToSharedOwnership { get; private set; }

    public string ExemptionJustification { get; private set; }

    public YesNoType ModernMethodsOfConstruction { get; private set; }

    public ModernMethodsConstructionCategoriesType ModernMethodsConstructionCategory { get; private set; }

    public void SetHomeTypeId(string homeTypeId)
    {
        Id = homeTypeId;
    }

    public GeneralHomeTypeData GenerateHomeTypeDetails()
    {
        Name = $"IT-General-{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)}";
        return this;
    }

    public GeneralHomeTypeData GenerateInformation()
    {
        NumberOfHomes = 4;
        NumberOfBedrooms = 3;
        MaximumOccupancy = 10;
        NumberOfStoreys = 1;
        return this;
    }

    public GeneralHomeTypeData GenerateMoveOnAccommodation()
    {
        MoveOnAccommodation = YesNoType.Yes;
        return this;
    }

    public GeneralHomeTypeData GenerateBuildingInformation()
    {
        BuildingType = BuildingType.House;
        return this;
    }

    public GeneralHomeTypeData GenerateCustomBuild()
    {
        CustomBuild = YesNoType.Yes;
        return this;
    }

    public GeneralHomeTypeData GenerateFacilityType()
    {
        FacilityType = FacilityType.SelfContainedFacilities;
        return this;
    }

    public GeneralHomeTypeData GenerateAccessibilityStandards()
    {
        AccessibilityStandards = YesNoType.Yes;
        return this;
    }

    public GeneralHomeTypeData GenerateAccessibilityCategory()
    {
        AccessibilityCategory = AccessibilityCategoryType.VisitableDwellings;
        return this;
    }

    public GeneralHomeTypeData GenerateFloorArea()
    {
        FloorArea = 125.59m;
        MeetSpaceStandards = YesNoType.No;
        return this;
    }

    public GeneralHomeTypeData GenerateFloorAreaStandards()
    {
        SpaceStandards = NationallyDescribedSpaceStandardType.BedroomWidth;
        return this;
    }

    public GeneralHomeTypeData GenerateAffordableRent()
    {
        MarketValue = 5000000;
        MarketRent = 1000;
        ProspectiveRent = 750;
        Exceeds80PercentOfMarketRent = YesNoType.Yes;
        ProspectiveRentPercentage = "75%";
        return this;
    }

    public GeneralHomeTypeData GenerateExemptFromTheRightToSharedOwnership()
    {
        ExemptFromTheRightToSharedOwnership = YesNoType.Yes;
        return this;
    }

    public GeneralHomeTypeData GenerateExemptionJustification()
    {
        ExemptionJustification = "this is some justification";
        return this;
    }

    public GeneralHomeTypeData GenerateModernMethodsOfConstruction()
    {
        ModernMethodsOfConstruction = YesNoType.Yes;
        return this;
    }

    public GeneralHomeTypeData GenerateModernMethodsConstructionCategory()
    {
        ModernMethodsConstructionCategory = ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems;
        return this;
    }
}
