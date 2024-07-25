using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.Common.CRM.Model;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.BuildActivityTypeMapperTests;

public class ToDtoTests
{
    [Theory]
    [InlineData(BuildActivityType.Undefined, null, null)]
    [InlineData(BuildActivityType.AcquisitionAndWorks, (int)invln_NewBuildActivityType.AcquisitionandWorks, null)]
    [InlineData(BuildActivityType.OffTheShelf, (int)invln_NewBuildActivityType.OffTheShelf, null)]
    [InlineData(BuildActivityType.WorksOnly, (int)invln_NewBuildActivityType.WorksOnly, null)]
    [InlineData(BuildActivityType.LandInclusivePackage, (int)invln_NewBuildActivityType.LandInclusivePackagepackagedeal, null)]
    [InlineData(BuildActivityType.Regeneration, (int)invln_NewBuildActivityType.Regeneration, null)]
    [InlineData(BuildActivityType.AcquisitionAndWorksRehab, null, (int)invln_RehabActivityType.AcquisitionandWorksrehab)]
    [InlineData(BuildActivityType.Conversion, null, (int)invln_RehabActivityType.Conversion)]
    [InlineData(BuildActivityType.ExistingSatisfactory, null, (int)invln_RehabActivityType.ExistingSatisfactory)]
    [InlineData(BuildActivityType.LeaseAndRepair, null, (int)invln_RehabActivityType.LeaseandRepair)]
    [InlineData(BuildActivityType.PurchaseAndRepair, null, (int)invln_RehabActivityType.PurchaseandRepair)]
    [InlineData(BuildActivityType.Reimprovement, null, (int)invln_RehabActivityType.Reimprovement)]
    [InlineData(BuildActivityType.WorksOnlyRehab, null, (int)invln_RehabActivityType.WorksOnly)]
    public void Should(BuildActivityType buildActivityType, int? expectedNewBuildActivityType, int? expectedRehabBuildActivityType)
    {
        // given && when
        var (newBuildActivityType, rehabBuildActivityType) = BuildActivityTypeMapper.ToDto(buildActivityType);

        // then
        newBuildActivityType.Should().Be(expectedNewBuildActivityType);
        rehabBuildActivityType.Should().Be(expectedRehabBuildActivityType);
    }
}
