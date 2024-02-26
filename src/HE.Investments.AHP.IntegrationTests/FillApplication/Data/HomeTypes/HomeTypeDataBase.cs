using System.Globalization;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.AHP.IntegrationTests.Extensions;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data.HomeTypes;

public abstract class HomeTypeDataBase<THomeTypeData> : INestedItemData
    where THomeTypeData : HomeTypeDataBase<THomeTypeData>
{
    public string Id { get; protected set; }

    public string Name { get; protected set; }

    public abstract HousingType HousingType { get; }

    public int NumberOfHomes { get; protected set; }

    public int NumberOfBedrooms { get; protected set; }

    public int MaximumOccupancy { get; protected set; }

    public int NumberOfStoreys { get; protected set; }

    public BuildingType BuildingType { get; protected set; }

    public YesNoType CustomBuild { get; protected set; }

    public FacilityType FacilityType { get; protected set; }

    public YesNoType AccessibilityStandards { get; protected set; }

    public AccessibilityCategoryType AccessibilityCategory { get; protected set; }

    public decimal FloorArea { get; protected set; }

    public YesNoType MeetSpaceStandards { get; protected set; }

    public NationallyDescribedSpaceStandardType SpaceStandards { get; protected set; }

    public int MarketValue { get; protected set; }

    public decimal MarketRent { get; protected set; }

    public decimal ProspectiveRent { get; protected set; }

    public string ProspectiveRentPercentage { get; protected set; }

    public YesNoType Exceeds80PercentOfMarketRent { get; protected set; }

    public YesNoType ExemptFromTheRightToSharedOwnership { get; protected set; }

    public string ExemptionJustification { get; protected set; }

    public YesNoType ModernMethodsOfConstruction { get; protected set; }

    public ModernMethodsConstructionCategoriesType ModernMethodsConstructionCategory { get; protected set; }

    protected abstract THomeTypeData HomeType { get; }

    public abstract THomeTypeData GenerateHomeTypeDetails();

    public THomeTypeData GenerateInformation()
    {
        NumberOfHomes = 4;
        NumberOfBedrooms = 3;
        MaximumOccupancy = 10;
        NumberOfStoreys = 1;
        return HomeType;
    }

    public THomeTypeData GenerateBuildingInformation()
    {
        BuildingType = BuildingType.House;
        return HomeType;
    }

    public THomeTypeData GenerateCustomBuild()
    {
        CustomBuild = YesNoType.Yes;
        return HomeType;
    }

    public THomeTypeData GenerateFacilityType()
    {
        FacilityType = FacilityType.SelfContainedFacilities;
        return HomeType;
    }

    public THomeTypeData GenerateAccessibilityStandards()
    {
        AccessibilityStandards = YesNoType.Yes;
        return HomeType;
    }

    public THomeTypeData GenerateAccessibilityCategory()
    {
        AccessibilityCategory = AccessibilityCategoryType.VisitableDwellings;
        return HomeType;
    }

    public THomeTypeData GenerateFloorArea()
    {
        FloorArea = 125.59m;
        MeetSpaceStandards = YesNoType.No;
        return HomeType;
    }

    public THomeTypeData GenerateFloorAreaStandards()
    {
        SpaceStandards = NationallyDescribedSpaceStandardType.BedroomWidth;
        return HomeType;
    }

    public THomeTypeData GenerateAffordableRent()
    {
        MarketValue = 5000000;
        MarketRent = 1000;
        ProspectiveRent = 750;
        Exceeds80PercentOfMarketRent = YesNoType.Yes;
        ProspectiveRentPercentage = "75%";
        return HomeType;
    }

    public THomeTypeData GenerateExemptFromTheRightToSharedOwnership()
    {
        ExemptFromTheRightToSharedOwnership = YesNoType.Yes;
        return HomeType;
    }

    public THomeTypeData GenerateExemptionJustification()
    {
        ExemptionJustification = nameof(ExemptionJustification).WithTimestampPrefix();
        return HomeType;
    }

    public THomeTypeData GenerateModernMethodsOfConstruction()
    {
        ModernMethodsOfConstruction = YesNoType.Yes;
        return HomeType;
    }

    public THomeTypeData GenerateModernMethodsConstructionCategory()
    {
        ModernMethodsConstructionCategory = ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems;
        return HomeType;
    }

    protected static string GenerateDateString() => DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
}
