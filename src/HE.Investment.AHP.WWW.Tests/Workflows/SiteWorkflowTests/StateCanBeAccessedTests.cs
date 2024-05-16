using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests;

public class StateCanBeAccessedTests
{
    [Theory]
    [InlineData(SiteWorkflowState.Start, true)]
    [InlineData(SiteWorkflowState.Name, true)]
    [InlineData(SiteWorkflowState.LocalAuthoritySearch, true)]
    [InlineData(SiteWorkflowState.LocalAuthorityResult, true)]
    [InlineData(SiteWorkflowState.LocalAuthorityConfirm, true)]
    [InlineData(SiteWorkflowState.LocalAuthorityReset, true)]
    [InlineData(SiteWorkflowState.PlanningStatus, true)]
    [InlineData(SiteWorkflowState.PlanningDetails, true)]
    [InlineData(SiteWorkflowState.LandRegistry, false)]
    [InlineData(SiteWorkflowState.NationalDesignGuide, true)]
    [InlineData(SiteWorkflowState.BuildingForHealthyLife, true)]
    [InlineData(SiteWorkflowState.NumberOfGreenLights, false)]
    [InlineData(SiteWorkflowState.LandAcquisitionStatus, true)]
    [InlineData(SiteWorkflowState.TenderingStatus, true)]
    [InlineData(SiteWorkflowState.ContractorDetails, false)]
    [InlineData(SiteWorkflowState.IntentionToWorkWithSme, false)]
    [InlineData(SiteWorkflowState.StrategicSite, true)]
    [InlineData(SiteWorkflowState.SiteType, true)]
    [InlineData(SiteWorkflowState.RuralClassification, true)]
    [InlineData(SiteWorkflowState.EnvironmentalImpact, true)]
    [InlineData(SiteWorkflowState.MmcUsing, true)]
    [InlineData(SiteWorkflowState.MmcFutureAdoption, false)]
    [InlineData(SiteWorkflowState.MmcInformation, false)]
    [InlineData(SiteWorkflowState.MmcCategories, false)]
    [InlineData(SiteWorkflowState.Mmc3DCategory, false)]
    [InlineData(SiteWorkflowState.Mmc2DCategory, false)]
    [InlineData(SiteWorkflowState.Procurements, true)]
    [InlineData(SiteWorkflowState.CheckAnswers, true)]
    public async Task ShouldReturnValue_WhenMethodCalledForDefaults(SiteWorkflowState state, bool expectedResult)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedResult);
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledFor()
    {
        // given
        var planningDetails = new SitePlanningDetails(SitePlanningStatus.DetailedPlanningApplicationSubmitted, IsLandRegistryTitleNumberRegistered: true);
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, planningDetails: planningDetails);

        // when
        var result = await workflow.StateCanBeAccessed(SiteWorkflowState.LandRegistry);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(SiteTenderingStatus.ConditionalWorksContract)]
    [InlineData(SiteTenderingStatus.UnconditionalWorksContract)]
    public async Task ShouldReturnTrue_WhenMethodCalledForContractorDetails(SiteTenderingStatus status)
    {
        // given
        var details = new SiteTenderingStatusDetails(status, null, null, null);
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, tenderingStatusDetails: details);

        // when
        var result = await workflow.StateCanBeAccessed(SiteWorkflowState.ContractorDetails);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(SiteTenderingStatus.TenderForWorksContract)]
    [InlineData(SiteTenderingStatus.ContractingHasNotYetBegun)]
    public async Task ShouldReturnTrue_WhenMethodCalledForIntentionToWorkWithSme(SiteTenderingStatus status)
    {
        // given
        var details = new SiteTenderingStatusDetails(status, null, null, null);
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, tenderingStatusDetails: details);

        // when
        var result = await workflow.StateCanBeAccessed(SiteWorkflowState.IntentionToWorkWithSme);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledForNumberOfGreenLightsAndBuildingForHealthyLifeIsYes()
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(SiteWorkflowState.Start, buildingForHealthyLife: BuildingForHealthyLifeType.Yes);

        // when
        var result = await workflow.StateCanBeAccessed(SiteWorkflowState.NumberOfGreenLights);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledForMmcFutureAdoptionForNoSiteUsingModernMethodsOfConstruction()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(SiteUsingModernMethodsOfConstruction.No);
        await Test(SiteWorkflowState.MmcFutureAdoption, modernMethodsOfConstruction);
    }

    [Theory]
    [InlineData(SiteUsingModernMethodsOfConstruction.Yes, true)]
    [InlineData(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes, true)]
    [InlineData(SiteUsingModernMethodsOfConstruction.No, false)]
    public async Task ShouldReturnTrue_WhenMethodCalledForMmcInformationForSomeSiteUsingModernMethodsOfConstruction(SiteUsingModernMethodsOfConstruction usingModernMethodsOfConstruction, bool expectedCanBeAccessed)
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(usingModernMethodsOfConstruction);
        await Test(SiteWorkflowState.MmcInformation, modernMethodsOfConstruction, expectedCanBeAccessed);
    }

    [Theory]
    [InlineData(SiteUsingModernMethodsOfConstruction.Yes, true)]
    [InlineData(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes, false)]
    public async Task ShouldReturnTrue_WhenMethodCalledForMmcCategoriesForSomeSiteUsingModernMethodsOfConstruction(SiteUsingModernMethodsOfConstruction usingModernMethodsOfConstruction, bool expectedCanBeAccessed)
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(usingModernMethodsOfConstruction);
        await Test(SiteWorkflowState.MmcCategories, modernMethodsOfConstruction, expectedCanBeAccessed);
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledForMmc3DCategoryForSelected3DCategory()
    {
        IList<ModernMethodsConstructionCategoriesType> modernMethodsConstructionCategories = [
            ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
        ];

        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            modernMethodsConstructionCategories);

        await Test(SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnTrue_WhenMethodCalledForMmc2DCategoryForSelected2DCategory()
    {
        IList<ModernMethodsConstructionCategoriesType> modernMethodsConstructionCategories = [
            ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
        ];

        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            modernMethodsConstructionCategories);

        await Test(SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction);
    }

    private static async Task Test(
        SiteWorkflowState state,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null,
        bool expectedCanBeAccessed = true)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            SiteWorkflowState.Start,
            modernMethodsOfConstruction: modernMethodsOfConstruction);

        // when
        var result = await workflow.StateCanBeAccessed(state);

        // then
        result.Should().Be(expectedCanBeAccessed);
    }
}
