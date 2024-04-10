using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.Entities.ModernMethodsConstructionSegmentEntityTests;

public class IsCompletedTests
{
    [Fact]
    public void ShouldReturnTrue_WhenModernMethodsConstructionIsNotApplied()
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder()
            .WithMmcIsApplied(YesNoType.No)
            .Build();

        // when
        var result = testCandidate.IsCompleted(HousingType.General, Tenure.AffordableRent);

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenModernMethodsConstructionAppliedIsNotSelected()
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder().Build();

        // when
        var result = testCandidate.IsCompleted(HousingType.General, Tenure.AffordableRent);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_WhenModernMethodsConstructionIsAppliedButNoCategoryIsSelected()
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder()
            .WithMmcIsApplied(YesNoType.Yes)
            .Build();

        // when
        var result = testCandidate.IsCompleted(HousingType.General, Tenure.AffordableRent);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenModernMethodsConstructionIsAppliedAndSomeCategoryIsSelected()
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder()
            .WithMmcIsApplied(YesNoType.Yes)
            .WithMmcCategories(ModernMethodsConstructionCategoriesType.Category3PreManufacturedComponentNonSystemizedPrimaryStructure)
            .Build();

        // when
        var result = testCandidate.IsCompleted(HousingType.General, Tenure.AffordableRent);

        // then
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems)]
    [InlineData(ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems)]
    public void ShouldReturnFalse_WhenModernMethodsConstructionIsAppliedAndSubCategoryIsNotSelected(ModernMethodsConstructionCategoriesType category)
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder()
            .WithMmcIsApplied(YesNoType.Yes)
            .WithMmcCategories(category)
            .Build();

        // when
        var result = testCandidate.IsCompleted(HousingType.General, Tenure.AffordableRent);

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenModernMethodsConstructionIsAppliedAndSubCategoryIsSelected()
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder()
            .WithMmcIsApplied(YesNoType.Yes)
            .WithMmcCategories(
                ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural)
            .WithMmc3DCategories(ModernMethodsConstruction3DSubcategoriesType.StructuralChassisOnly)
            .WithMmc2DCategories(ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation)
            .Build();

        // when
        var result = testCandidate.IsCompleted(HousingType.General, Tenure.AffordableRent);

        // then
        result.Should().BeTrue();
    }
}
