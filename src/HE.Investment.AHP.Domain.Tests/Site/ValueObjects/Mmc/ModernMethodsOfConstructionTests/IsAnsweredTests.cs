using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Site.ValueObjects.Mmc.ModernMethodsOfConstructionTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenProvidedValid2DAnd3DCategories()
    {
        // given
        var details = ModernMethodsOfConstructionBuilder.New().BuildValid2DAnd3DCategories();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnTrue_WhenProvidedValidOtherCategories()
    {
        // given
        var details = ModernMethodsOfConstructionBuilder.New().BuildValidOtherCategories();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenCategoryNotProvided()
    {
        // given
        var details = ModernMethodsOfConstructionBuilder.New().Build();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_When2DCategorySelectedButSubcategoryNotProvided()
    {
        // given
        var details = ModernMethodsOfConstructionBuilder.New().With2DCategory().Build();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnFalse_When3DCategorySelectedButSubcategoryNotProvided()
    {
        // given
        var details = ModernMethodsOfConstructionBuilder.New().With3DCategory().Build();

        // when
        var result = details.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
