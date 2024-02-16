using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SiteWorkflowTests.Mmc;

public class NextStateTests
{
    [Theory]
    [InlineData(SiteWorkflowState.EnvironmentalImpact, SiteWorkflowState.MmcUsing)]
    [InlineData(SiteWorkflowState.MmcUsing, SiteWorkflowState.Procurements)]
    [InlineData(SiteWorkflowState.MmcFutureAdoption, SiteWorkflowState.Procurements)]
    [InlineData(SiteWorkflowState.MmcInformation, SiteWorkflowState.MmcCategories)]
    [InlineData(SiteWorkflowState.MmcCategories, SiteWorkflowState.Procurements)]
    [InlineData(SiteWorkflowState.Mmc3DCategory, SiteWorkflowState.Procurements)]
    [InlineData(SiteWorkflowState.Mmc2DCategory, SiteWorkflowState.Procurements)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        await TestContinue(current, expectedNext);
    }

    [Theory]
    [InlineData(SiteWorkflowState.Procurements, SiteWorkflowState.MmcCategories)]
    [InlineData(SiteWorkflowState.Mmc3DCategory, SiteWorkflowState.MmcCategories)]
    [InlineData(SiteWorkflowState.MmcCategories, SiteWorkflowState.MmcInformation)]
    [InlineData(SiteWorkflowState.MmcInformation, SiteWorkflowState.MmcUsing)]
    [InlineData(SiteWorkflowState.MmcFutureAdoption, SiteWorkflowState.MmcUsing)]
    public async Task ShouldReturnNextState_WhenBackTriggerExecuted(SiteWorkflowState current, SiteWorkflowState expectedNext)
    {
        await TestBack(current, expectedNext);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithNoModernMethodsOfConstruction()
    {
        await TestContinue(
            SiteWorkflowState.MmcUsing,
            SiteWorkflowState.MmcFutureAdoption,
            new SiteModernMethodsOfConstruction(SiteUsingModernMethodsOfConstruction.No));
    }

    [Theory]
    [InlineData(SiteUsingModernMethodsOfConstruction.Yes)]
    [InlineData(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithModernMethodsOfConstruction(
        SiteUsingModernMethodsOfConstruction usingModernMethodsOfConstruction)
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(usingModernMethodsOfConstruction);

        await TestContinue(SiteWorkflowState.MmcUsing, SiteWorkflowState.MmcInformation, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForMmcCategoriesWithSelected3DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType> { ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems, });

        await TestContinue(SiteWorkflowState.MmcCategories, SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForMmcCategoriesWithSelected2DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType> { ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems, });

        await TestContinue(SiteWorkflowState.MmcCategories, SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedForMmcCategoriesWithSelected3dAnd2DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType>
            {
                ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
            });

        await TestContinue(SiteWorkflowState.MmcCategories, SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnNextState_WhenContinueTriggerExecutedWithSelected2DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType> { ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems, });

        await TestContinue(SiteWorkflowState.Mmc3DCategory, SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnMmc2DCategory_WhenBackTriggerExecutedAndSelected2DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType> { ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems, });

        await TestBack(SiteWorkflowState.Procurements, SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnMmc3DCategory_WhenBackTriggerExecutedForProcurementsAndSelected3DAnd2DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType>
            {
                ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
            });

        await TestBack(SiteWorkflowState.Procurements, SiteWorkflowState.Mmc2DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnMmc3DCategory_WhenBackTriggerExecutedForProcurementsAndSelected3DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType> { ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems, });

        await TestBack(SiteWorkflowState.Procurements, SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnMmcFutureAdoption_WhenBackTriggerExecutedAndNoModernMethodsOfConstruction()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(SiteUsingModernMethodsOfConstruction.No);

        await TestBack(SiteWorkflowState.Procurements, SiteWorkflowState.MmcFutureAdoption, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnMmc3DCategory_WhenBackTriggerExecutedForMmc2DCategoryAndSelected3DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType> { ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems, });

        await TestBack(SiteWorkflowState.Mmc2DCategory, SiteWorkflowState.Mmc3DCategory, modernMethodsOfConstruction);
    }

    [Fact]
    public async Task ShouldReturnMmcCategories_WhenBackTriggerExecutedForMmc2DCategoryAndNotSelected3DOr2DCategory()
    {
        var modernMethodsOfConstruction = new SiteModernMethodsOfConstruction(
            SiteUsingModernMethodsOfConstruction.Yes,
            new List<ModernMethodsConstructionCategoriesType>
            {
                ModernMethodsConstructionCategoriesType.Category3PreManufacturedComponentNonSystemizedPrimaryStructure,
            });

        await TestBack(SiteWorkflowState.Mmc2DCategory, SiteWorkflowState.MmcCategories, modernMethodsOfConstruction);
    }

    private static async Task TestBack(
        SiteWorkflowState currentState,
        SiteWorkflowState expectedState,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(currentState, modernMethodsOfConstruction: modernMethodsOfConstruction);

        // when
        var result = await workflow.NextState(Trigger.Back);

        // then
        result.Should().Be(expectedState);
    }

    private static async Task TestContinue(
        SiteWorkflowState currentState,
        SiteWorkflowState expectedState,
        SiteModernMethodsOfConstruction? modernMethodsOfConstruction = null)
    {
        // given
        var workflow = SiteWorkflowFactory.BuildWorkflow(
            currentState,
            modernMethodsOfConstruction: modernMethodsOfConstruction);

        // when
        var result = await workflow.NextState(Trigger.Continue);

        // then
        result.Should().Be(expectedState);
    }
}
