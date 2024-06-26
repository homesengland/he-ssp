using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Contract.Enum;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.Entities.ModernMethodsConstructionSegmentEntityTests;

public class ChangeModernMethodsConstructionAppliedTests
{
    [Fact]
    public void ShouldClearCategories_WhenMmcIsNotUsed()
    {
        // given
        var testCandidate = new ModernMethodsConstructionSegmentEntityBuilder()
            .WithMmcIsApplied(YesNoType.Yes)
            .WithMmcCategories(
                ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural)
            .WithMmc3DCategories(ModernMethodsConstruction3DSubcategoriesType.StructuralChassisOnly)
            .WithMmc2DCategories(ModernMethodsConstruction2DSubcategoriesType.FurtherEnhancedConsolidation)
            .Build();

        // when
        testCandidate.ChangeModernMethodsConstructionApplied(YesNoType.No);

        // then
        testCandidate.IsModified.Should().BeTrue();
        testCandidate.ModernMethodsConstructionCategories.Should().BeEmpty();
        testCandidate.ModernMethodsConstruction3DSubcategories.Should().BeEmpty();
        testCandidate.ModernMethodsConstruction2DSubcategories.Should().BeEmpty();
    }
}
