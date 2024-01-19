using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investments.IntegrationTestsFramework.Data;

namespace HE.Investments.AHP.IntegrationTests.FillApplication.Data.HomeTypes;

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
        MarketRent = homeType.MarketRent;
        ProspectiveRent = homeType.ProspectiveRent;
        ProspectiveRentPercentage = homeType.ProspectiveRentPercentage;
        Exceeds80PercentOfMarketRent = homeType.Exceeds80PercentOfMarketRent;
        ExemptFromTheRightToSharedOwnership = homeType.ExemptFromTheRightToSharedOwnership;
        ExemptionJustification = homeType.ExemptionJustification;
        ModernMethodsOfConstruction = homeType.ModernMethodsOfConstruction;
        ModernMethodsConstructionCategory = homeType.ModernMethodsConstructionCategory;
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
        DesignFile = new FileEntry("design_plan.pdf", "application/pdf", new MemoryStream(new byte[] { 1, 2, 3 }));
        DesignPlanInformation = GenerateTextField(nameof(DesignPlanInformation));
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
        MoveOnArrangements = GenerateTextField(nameof(MoveOnArrangements));
        return this;
    }

    public HomesForDisabledPeopleData GenerateExitPlan()
    {
        ExitPlan = GenerateTextField(nameof(ExitPlan));
        return this;
    }

    public HomesForDisabledPeopleData GenerateTypologyLocationAndDesign()
    {
        TypologyLocationAndDesign = GenerateTextField(nameof(TypologyLocationAndDesign));
        return this;
    }

    public HomesForDisabledPeopleData GeneratePeopleGroupForSpecificDesignFeatures()
    {
        PeopleGroupForSpecificDesignFeatures = PeopleGroupForSpecificDesignFeaturesType.DisabledPeople;
        return this;
    }
}
