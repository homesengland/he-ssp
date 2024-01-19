using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;

namespace HE.Investment.AHP.WWW.Tests.TestDataBuilders;

public class HomeTypeTestDataBuilder
{
    private Tenure _tenure = Tenure.AffordableRent;

    private HousingType _housingType = HousingType.General;

    private RevenueFundingType _revenueFundingType = RevenueFundingType.RevenueFundingNotNeeded;

    private YesNoType _shortStayAccommodation = YesNoType.Yes;

    private BuildingType _buildingType = BuildingType.House;

    private YesNoType _accessibilityStandards = YesNoType.No;

    private YesNoType _spaceStandardsMet = YesNoType.Yes;

    private YesNoType _exemptFromTheRightToSharedOwnership = YesNoType.No;

    private bool _isProspectiveRentIneligible;

    private YesNoType _modernMethodsConstructionApplied = YesNoType.Yes;

    private IList<ModernMethodsConstructionCategoriesType> _modernMethodsConstructionCategories = new List<ModernMethodsConstructionCategoriesType>();

    public HomeTypeTestDataBuilder WithTenure(Tenure tenure)
    {
        _tenure = tenure;
        return this;
    }

    public HomeTypeTestDataBuilder WithHousingType(HousingType housingType)
    {
        _housingType = housingType;
        return this;
    }

    public HomeTypeTestDataBuilder WithRevenueFundingType(RevenueFundingType revenueFundingType)
    {
        _revenueFundingType = revenueFundingType;
        return this;
    }

    public HomeTypeTestDataBuilder WithShortStayAccommodation(YesNoType shortStayAccommodation)
    {
        _shortStayAccommodation = shortStayAccommodation;
        return this;
    }

    public HomeTypeTestDataBuilder WithBuildingType(BuildingType buildingType)
    {
        _buildingType = buildingType;
        return this;
    }

    public HomeTypeTestDataBuilder WithAccessibilityStandards(YesNoType accessibilityStandards)
    {
        _accessibilityStandards = accessibilityStandards;
        return this;
    }

    public HomeTypeTestDataBuilder WithSpaceStandardsMet(YesNoType spaceStandardsMet)
    {
        _spaceStandardsMet = spaceStandardsMet;
        return this;
    }

    public HomeTypeTestDataBuilder WithExemptFromTheRightToSharedOwnership(YesNoType exemptFromTheRightToSharedOwnership)
    {
        _exemptFromTheRightToSharedOwnership = exemptFromTheRightToSharedOwnership;
        return this;
    }

    public HomeTypeTestDataBuilder WithProspectiveRentIneligible()
    {
        _isProspectiveRentIneligible = true;
        return this;
    }

    public HomeTypeTestDataBuilder WithModernMethodsConstructionApplied(YesNoType modernMethodsConstructionApplied)
    {
        _modernMethodsConstructionApplied = modernMethodsConstructionApplied;
        return this;
    }

    public HomeTypeTestDataBuilder WithModernMethodsConstructionCategories(IList<ModernMethodsConstructionCategoriesType> modernMethodsConstructionCategories)
    {
        _modernMethodsConstructionCategories = modernMethodsConstructionCategories;
        return this;
    }

    public HomeType Build()
    {
        return new HomeType(
            HomeTypeId.From("home-type-id"),
            "My Home Type",
            _housingType,
            _tenure,
            new HomeTypeConditionals(
                _shortStayAccommodation,
                _revenueFundingType,
                _buildingType,
                _accessibilityStandards,
                _spaceStandardsMet,
                _exemptFromTheRightToSharedOwnership,
                _isProspectiveRentIneligible,
                _modernMethodsConstructionApplied,
                _modernMethodsConstructionCategories));
    }
}
