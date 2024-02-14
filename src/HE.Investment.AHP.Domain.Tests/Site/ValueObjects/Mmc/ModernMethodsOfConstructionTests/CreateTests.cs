using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects.Mmc;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Mmc.ModernMethodsOfConstructionTests;

public class CreateTests
{
    private readonly ModernMethodsOfConstruction _testData = new(
        new[]
        {
            ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
            ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
        }.ToList(),
        new[] { ModernMethodsConstruction2DSubcategoriesType.EnhancedConsolidation }.ToList(),
        new[] { ModernMethodsConstruction3DSubcategoriesType.StructuralChassisAndInternallyFittedOut }.ToList());

    [Fact]
    public void ShouldCreate_WhenAllDataProvided()
    {
        // given && when
        var result = new ModernMethodsOfConstruction(
            _testData.ModernMethodsConstructionCategories,
            _testData.ModernMethodsConstruction2DSubcategories,
            _testData.ModernMethodsConstruction3DSubcategories);

        // then
        result.Should().NotBeNull();
        result.ModernMethodsConstructionCategories.Should().BeEquivalentTo(_testData.ModernMethodsConstructionCategories);
        result.ModernMethodsConstruction3DSubcategories.Should().BeEquivalentTo(_testData.ModernMethodsConstruction3DSubcategories);
        result.ModernMethodsConstruction2DSubcategories.Should().BeEquivalentTo(_testData.ModernMethodsConstruction2DSubcategories);
    }

    [Fact]
    public void ShouldThrowException_When2DCategoryIsMissingForSelected2DSubcategory()
    {
        // given && when
        var result = () => new ModernMethodsOfConstruction(
            new[]
            {
                ModernMethodsConstructionCategoriesType.Category1PreManufacturing3DPrimaryStructuralSystems,
                ModernMethodsConstructionCategoriesType.Category4AdditiveManufacturingStructuringAndNonStructural,
            }.ToList(),
            _testData.ModernMethodsConstruction2DSubcategories,
            _testData.ModernMethodsConstruction3DSubcategories);

        // then
        result.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldThrowException_When3DCategoryIsMissingForSelected3DSubcategory()
    {
        // given && when
        var result = () => new ModernMethodsOfConstruction(
            new[]
            {
                ModernMethodsConstructionCategoriesType.Category3PreManufacturedComponentNonSystemizedPrimaryStructure,
                ModernMethodsConstructionCategoriesType.Category2PreManufacturing2DPrimaryStructuralSystems,
            }.ToList(),
            _testData.ModernMethodsConstruction2DSubcategories,
            _testData.ModernMethodsConstruction3DSubcategories);

        // then
        result.Should().Throw<DomainValidationException>();
    }
}
