using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Tests.Workflows.HomeTypesWorkflowTests;

public class ModernMethodsConstructionTransitionTests
{
    [Theory]
    [InlineData(Tenure.AffordableRent, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)]
    [InlineData(Tenure.SocialRent, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)]
    [InlineData(Tenure.SharedOwnership, HomeTypesWorkflowState.SharedOwnership)]
    [InlineData(Tenure.RentToBuy, HomeTypesWorkflowState.RentToBuy)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities, HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    [InlineData(Tenure.OlderPersonsSharedOwnership, HomeTypesWorkflowState.OlderPersonsSharedOwnership)]
    public async Task ShouldSkipMmcSection_WhenSiteIsUsingMmcAndTenureIs(Tenure tenure, HomeTypesWorkflowState startingWorkflowType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.Yes)
            .WithExemptFromTheRightToSharedOwnership(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, startingWorkflowType);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.CheckAnswers);
        states[1].Should().Be(startingWorkflowType);
    }

    [Theory]
    [InlineData(Tenure.AffordableRent, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)]
    [InlineData(Tenure.SocialRent, HomeTypesWorkflowState.ExemptFromTheRightToSharedOwnership)]
    [InlineData(Tenure.SharedOwnership, HomeTypesWorkflowState.SharedOwnership)]
    [InlineData(Tenure.RentToBuy, HomeTypesWorkflowState.RentToBuy)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities, HomeTypesWorkflowState.HomeOwnershipDisabilities)]
    [InlineData(Tenure.OlderPersonsSharedOwnership, HomeTypesWorkflowState.OlderPersonsSharedOwnership)]
    public async Task ShouldSkipMmcSection_WhenSiteIsNotUsingMmcAndTenureIs(Tenure tenure, HomeTypesWorkflowState startingWorkflowType)
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithTenure(tenure)
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.No)
            .WithExemptFromTheRightToSharedOwnership(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, startingWorkflowType);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.CheckAnswers);
        states[1].Should().Be(startingWorkflowType);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenModernMethodsConstructionIsYes()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionApplied(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ModernMethodsConstruction);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ModernMethodsConstructionCategories);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenModernMethodsConstructionsIsYes()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionApplied(YesNoType.Yes)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ModernMethodsConstructionCategories);
        states[1].Should().Be(HomeTypesWorkflowState.ModernMethodsConstruction);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenModernMethodsConstructionsIsNo()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionApplied(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ModernMethodsConstruction);

        // when
        var state = await workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenModernMethodsConstructionsIsNo()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionApplied(YesNoType.No)
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var state = await workflow.NextState(Trigger.Back);

        // then
        state.Should().Be(HomeTypesWorkflowState.ModernMethodsConstruction);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenModernMethodsConstructionCategoriesOnlyContainsCategory1()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionCategories(new[] { ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems })
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ModernMethodsConstructionCategories);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenModernMethodsConstructionCategoriesOnlyContainsCategory1()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionCategories(new[] { ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems })
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ModernMethodsConstruction3DSubcategories);
        states[1].Should().Be(HomeTypesWorkflowState.ModernMethodsConstructionCategories);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenModernMethodsConstructionCategoriesOnlyContainsCategory2()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionCategories(new[] { ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems })
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ModernMethodsConstructionCategories);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Continue),
            await workflow.NextState(Trigger.Continue),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories);
        states[1].Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenModernMethodsConstructionCategoriesOnlyContainsCategory2()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionCategories(new[] { ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems })
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var states = new[]
        {
            await workflow.NextState(Trigger.Back),
            await workflow.NextState(Trigger.Back),
        };

        // then
        states[0].Should().Be(HomeTypesWorkflowState.ModernMethodsConstruction2DSubcategories);
        states[1].Should().Be(HomeTypesWorkflowState.ModernMethodsConstructionCategories);
    }

    [Fact]
    public async Task ShouldNavigateForward_WhenModernMethodsConstructionCategoriesNotContainsCategory1AndCategory2()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionCategories(new[]
            {
                ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural,
                ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
                ModernMethodsConstructionCategoriesType.Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements,
            })
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.ModernMethodsConstructionCategories);

        // when
        var state = await workflow.NextState(Trigger.Continue);

        // then
        state.Should().Be(HomeTypesWorkflowState.CheckAnswers);
    }

    [Fact]
    public async Task ShouldNavigateBackward_WhenModernMethodsConstructionCategoriesNotContainsCategory1AndCategory2()
    {
        // given
        var homeType = new HomeTypeTestDataBuilder()
            .WithSiteUsingMmc(SiteUsingModernMethodsOfConstruction.OnlyForSomeHomes)
            .WithModernMethodsConstructionCategories(new[]
            {
                ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural,
                ModernMethodsConstructionCategoriesType.Category6TraditionalBuildingProductLedSiteLabourReductionOrProductivityImprovements,
                ModernMethodsConstructionCategoriesType.Category7SiteProcessLedLabourReductionOrProductivityOrAssuranceImprovements,
            })
            .Build();
        var workflow = BuildWorkflow(homeType, HomeTypesWorkflowState.CheckAnswers);

        // when
        var state = await workflow.NextState(Trigger.Back);

        // then
        state.Should().Be(HomeTypesWorkflowState.ModernMethodsConstructionCategories);
    }

    private static HomeTypesWorkflow BuildWorkflow(HomeType homeType, HomeTypesWorkflowState state)
    {
        return new HomeTypesWorkflow(state, homeType, false);
    }
}
