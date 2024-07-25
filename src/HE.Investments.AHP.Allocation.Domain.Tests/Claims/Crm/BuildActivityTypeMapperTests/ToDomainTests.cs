using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.Common.CRM.Model;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.BuildActivityTypeMapperTests;

public class ToDomainTests
{
    [Fact]
    public void ShouldThrowException_WhenNewBuildAndRehabBuildActivityTypesAreNull()
    {
        // given && when
        var map = () => BuildActivityTypeMapper.ToDomain(null, null);

        // then
        map.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData((int)invln_NewBuildActivityType.AcquisitionandWorks, BuildActivityType.AcquisitionAndWorks)]
    [InlineData((int)invln_NewBuildActivityType.OffTheShelf, BuildActivityType.OffTheShelf)]
    [InlineData((int)invln_NewBuildActivityType.WorksOnly, BuildActivityType.WorksOnly)]
    [InlineData((int)invln_NewBuildActivityType.LandInclusivePackagepackagedeal, BuildActivityType.LandInclusivePackage)]
    [InlineData((int)invln_NewBuildActivityType.Regeneration, BuildActivityType.Regeneration)]
    public void ShouldMapBuildActivityType_WhenNewBuildActivityTypeIs(int newBuildActivityType, BuildActivityType expectedBuildActivityType)
    {
        // given && when
        var result = BuildActivityTypeMapper.ToDomain(newBuildActivityType, null);

        // then
        result.Should().Be(expectedBuildActivityType);
    }

    [Fact]
    public void ShouldThrowException_WhenNewBuildActivityTypeIsNotKnown()
    {
        // given && when
        var map = () => BuildActivityTypeMapper.ToDomain(858110005, null);

        // then
        map.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("newBuildActivityType");
    }

    [Theory]
    [InlineData((int)invln_RehabActivityType.AcquisitionandWorksrehab, BuildActivityType.AcquisitionAndWorksRehab)]
    [InlineData((int)invln_RehabActivityType.Conversion, BuildActivityType.Conversion)]
    [InlineData((int)invln_RehabActivityType.ExistingSatisfactory, BuildActivityType.ExistingSatisfactory)]
    [InlineData((int)invln_RehabActivityType.LeaseandRepair, BuildActivityType.LeaseAndRepair)]
    [InlineData((int)invln_RehabActivityType.PurchaseandRepair, BuildActivityType.PurchaseAndRepair)]
    [InlineData((int)invln_RehabActivityType.Reimprovement, BuildActivityType.Reimprovement)]
    [InlineData((int)invln_RehabActivityType.WorksOnly, BuildActivityType.WorksOnlyRehab)]
    public void ShouldMapBuildActivityType_WhenRehabBuildActivityTypeIs(int rehabBuildActivityType, BuildActivityType expectedBuildActivityType)
    {
        // given && when
        var result = BuildActivityTypeMapper.ToDomain(null, rehabBuildActivityType);

        // then
        result.Should().Be(expectedBuildActivityType);
    }

    [Fact]
    public void ShouldThrowException_WhenRehabBuildActivityTypeIsNotKnown()
    {
        // given && when
        var map = () => BuildActivityTypeMapper.ToDomain(null, 858110007);

        // then
        map.Should().Throw<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("rehabBuildActivityType");
    }
}
