using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.IntegrationTestsFramework.Data;
using HE.Investments.TestsUtils.Extensions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data.HomeTypes;

public class HomesForDisabledPeopleData : HomeTypeDataBase<HomesForDisabledPeopleData>
{
    public HomesForDisabledPeopleData()
    {
    }

    private HomesForDisabledPeopleData(string homeTypeId, string homeTypeName, GeneralHomeTypeData homeType)
    {
        Id = homeTypeId;
        Name = homeTypeName;
        NumberOfHomes = homeType.NumberOfHomes;
        NumberOfBedrooms = homeType.NumberOfBedrooms;
        MaximumOccupancy = homeType.MaximumOccupancy;
        NumberOfStoreys = homeType.NumberOfStoreys;
        BuildingType = homeType.BuildingType;
        CustomBuild = homeType.CustomBuild;
        FacilityType = homeType.FacilityType;
        AccessibilityStandards = homeType.AccessibilityStandards;
        AccessibilityCategory = homeType.AccessibilityCategory;
        FloorArea = homeType.FloorArea;
        MeetSpaceStandards = homeType.MeetSpaceStandards;
        SpaceStandards = homeType.SpaceStandards;
        MarketValue = homeType.MarketValue;
        MarketRentPerWeek = homeType.MarketRentPerWeek;
        RentPerWeek = homeType.RentPerWeek;
        ProspectiveRentPercentage = homeType.ProspectiveRentPercentage;
        Exceeds80PercentOfMarketRent = homeType.Exceeds80PercentOfMarketRent;
        ExemptFromTheRightToSharedOwnership = homeType.ExemptFromTheRightToSharedOwnership;
        ExemptionJustification = homeType.ExemptionJustification;
        ModernMethodsOfConstruction = homeType.ModernMethodsOfConstruction;
        MmcCategories = homeType.MmcCategories;
        Mmc3DSubcategory = homeType.Mmc3DSubcategory;
        Mmc2DSubcategory = homeType.Mmc2DSubcategory;
    }

    public override HousingType HousingType => HousingType.HomesForDisabledAndVulnerablePeople;

    public DisabledPeopleHousingType DisabledPeopleHousingType { get; private set; }

    public DisabledPeopleClientGroupType ClientGroup { get; private set; }

    public HappiDesignPrincipleType HappiDesignPrinciple { get; private set; }

    public FileEntry DesignFile { get; private set; }

    public string DesignPlanInformation { get; private set; }

    public YesNoType LocalCommissioningBodiesConsulted { get; private set; }

    public YesNoType ShortStayAccommodation { get; private set; }

    public RevenueFundingType RevenueFundingType { get; private set; }

    public RevenueFundingSourceType RevenueFundingSource { get; private set; }

    public string MoveOnArrangements { get; private set; }

    public string ExitPlan { get; private set; }

    public string TypologyLocationAndDesign { get; private set; }

    public PeopleGroupForSpecificDesignFeaturesType PeopleGroupForSpecificDesignFeatures { get; private set; }

    protected override HomesForDisabledPeopleData HomeType => this;

    public static HomesForDisabledPeopleData DuplicateFromGeneralHomeType(string homeTypeId, string homeTypeName, GeneralHomeTypeData homeType)
    {
        return new HomesForDisabledPeopleData(homeTypeId, homeTypeName, homeType);
    }

    public void SetHomeTypeId(string homeTypeId)
    {
        Id = homeTypeId;
    }

    public override HomesForDisabledPeopleData GenerateHomeTypeDetails()
    {
        Name = $"IT-Disabled-{GenerateDateString()}";
        return this;
    }

    public HomesForDisabledPeopleData GenerateDisabledPeopleHousingType()
    {
        DisabledPeopleHousingType = DisabledPeopleHousingType.DesignatedHomes;
        return this;
    }

    public HomesForDisabledPeopleData GenerateClientGroup()
    {
        ClientGroup = DisabledPeopleClientGroupType.PeopleWithAlcoholProblems;
        return this;
    }

    public HomesForDisabledPeopleData GenerateHappiDesignPrinciple()
    {
        HappiDesignPrinciple = HappiDesignPrincipleType.NoneOfThese;
        return this;
    }

    public HomesForDisabledPeopleData GenerateDesignPlans()
    {
        DesignFile = new FileEntry("design_plan.pdf", "application/pdf", new MemoryStream([1, 2, 3]));
        DesignPlanInformation = nameof(DesignPlanInformation).WithTimestampPrefix();
        return this;
    }

    public HomesForDisabledPeopleData GenerateSupportedHousingInformation()
    {
        LocalCommissioningBodiesConsulted = YesNoType.Yes;
        ShortStayAccommodation = YesNoType.Yes;
        RevenueFundingType = RevenueFundingType.RevenueFundingNeededAndIdentified;
        return this;
    }

    public HomesForDisabledPeopleData GenerateRevenueFundingSource()
    {
        RevenueFundingSource = RevenueFundingSourceType.SocialServicesDepartment;
        return this;
    }

    public HomesForDisabledPeopleData GenerateMoveOnArrangements()
    {
        MoveOnArrangements = nameof(MoveOnArrangements).WithTimestampPrefix();
        return this;
    }

    public HomesForDisabledPeopleData GenerateExitPlan()
    {
        ExitPlan = nameof(ExitPlan).WithTimestampPrefix();
        return this;
    }

    public HomesForDisabledPeopleData GenerateTypologyLocationAndDesign()
    {
        TypologyLocationAndDesign = nameof(TypologyLocationAndDesign).WithTimestampPrefix();
        return this;
    }

    public HomesForDisabledPeopleData GeneratePeopleGroupForSpecificDesignFeatures()
    {
        PeopleGroupForSpecificDesignFeatures = PeopleGroupForSpecificDesignFeaturesType.DisabledPeople;
        return this;
    }

    public override void PopulateAllData()
    {
        GenerateInformation();
        GenerateBuildingInformation();
        GenerateCustomBuild();
        GenerateFacilityType();
        GenerateAccessibilityStandards();
        GenerateAccessibilityCategory();
        GenerateFloorArea();
        GenerateFloorAreaStandards();
        GenerateAffordableRent();
        GenerateExemptFromTheRightToSharedOwnership();
        GenerateExemptionJustification();
        GenerateModernMethodsOfConstruction();
        GenerateModernMethodsConstructionCategories();
        GenerateDateString();
        GenerateHomeTypeDetails();
        GenerateDisabledPeopleHousingType();
        GenerateClientGroup();
        GenerateHappiDesignPrinciple();
        GenerateDesignPlans();
        GenerateSupportedHousingInformation();
        GenerateRevenueFundingSource();
        GenerateMoveOnArrangements();
        GenerateExitPlan();
        GenerateTypologyLocationAndDesign();
        GeneratePeopleGroupForSpecificDesignFeatures();
    }
}
