using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Crm.DeliveryPhaseCrmMapperTests;

public class MapToDtoTests : TestBase<DeliveryPhaseCrmMapper>
{
    [Fact]
    public void ShouldMapDeliveryPhaseBasicProperties()
    {
        // given
        var entity = new DeliveryPhaseEntityBuilder()
            .WithId("123123")
            .WithName("my phase")
            .WithReconfiguringExisting()
            .WithoutAcquisitionMilestone()
            .WithoutStartOnSiteMilestone()
            .WithoutCompletionMilestone()
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.Should().NotBeNull();
        result.id.Should().Be("123123");
        result.applicationId.Should().Be(entity.Application.Id.Value);
        result.name.Should().Be("my phase");
        result.isReconfigurationOfExistingProperties.Should().BeTrue();
        result.acquisitionDate.Should().BeNull();
        result.acquisitionPaymentDate.Should().BeNull();
        result.startOnSiteDate.Should().BeNull();
        result.startOnSitePaymentDate.Should().BeNull();
        result.completionDate.Should().BeNull();
        result.completionPaymentDate.Should().BeNull();
        result.numberOfHomes.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(TypeOfHomes.NewBuild, "newBuild")]
    [InlineData(TypeOfHomes.Rehab, "rehab")]
    public void ShouldMapTypeOfHomes(TypeOfHomes? typeOfHomes, string? expectedTypeOfHomes)
    {
        // given
        var entity = new DeliveryPhaseEntityBuilder()
            .WithTypeOfHomes(typeOfHomes)
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.typeOfHomes.Should().Be(expectedTypeOfHomes);
    }

    [Theory]
    [InlineData(BuildActivityType.AcquisitionAndWorksRehab, 858110000)]
    [InlineData(BuildActivityType.ExistingSatisfactory, 858110001)]
    [InlineData(BuildActivityType.PurchaseAndRepair, 858110002)]
    [InlineData(BuildActivityType.LeaseAndRepair, 858110003)]
    [InlineData(BuildActivityType.Reimprovement, 858110004)]
    [InlineData(BuildActivityType.Conversion, 858110005)]
    [InlineData(BuildActivityType.WorksOnlyRehab, 858110006)]
    [InlineData(BuildActivityType.RegenerationRehab, 858110007)]
    public void ShouldMapBuildActivity_WhenTypeOfHomesIsRehab(BuildActivityType buildActivityType, int expectedRehabBuildActivityType)
    {
        // given
        var entity = new DeliveryPhaseEntityBuilder()
            .WithRehabBuildActivity(buildActivityType)
            .WithoutStartOnSiteMilestone()
            .WithoutAcquisitionMilestone()
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.rehabBuildActivityType.Should().Be(expectedRehabBuildActivityType);
        result.newBuildActivityType.Should().BeNull();
    }

    [Theory]
    [InlineData(BuildActivityType.AcquisitionAndWorks, 858110000)]
    [InlineData(BuildActivityType.OffTheShelf, 858110001)]
    [InlineData(BuildActivityType.WorksOnly, 858110002)]
    [InlineData(BuildActivityType.LandInclusivePackage, 858110003)]
    [InlineData(BuildActivityType.Regeneration, 858110004)]
    public void ShouldMapBuildActivity_WhenTypeOfHomesIsNewBuild(BuildActivityType buildActivityType, int expectedNewBuildActivityType)
    {
        // given
        var entity = new DeliveryPhaseEntityBuilder()
            .WithNewBuildActivity(buildActivityType)
            .WithoutAcquisitionMilestone()
            .WithoutStartOnSiteMilestone()
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.newBuildActivityType.Should().Be(expectedNewBuildActivityType);
        result.rehabBuildActivityType.Should().BeNull();
    }

    [Fact]
    public void ShouldMapHomesToDeliver_WhenNumberOfHomesIsProvided()
    {
        // given
        var entity = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(0, "ht-1")
            .WithHomesToBeDelivered(12, "ht-2")
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.numberOfHomes.Should().HaveCount(2);
        result.numberOfHomes.Should().ContainKey("ht-1").WhoseValue.Should().Be(0);
        result.numberOfHomes.Should().ContainKey("ht-2").WhoseValue.Should().Be(12);
    }

    [Fact]
    public void ShouldMapMilestoneDates_WhenAllAreProvided()
    {
        // given
        var today = new DateOnly(2024, 01, 28);
        var entity = new DeliveryPhaseEntityBuilder()
            .WithAcquisitionMilestone(new AcquisitionMilestoneDetailsBuilder().WithAcquisitionDate(today)
                .WithPaymentDate(today.AddDays(1))
                .Build())
            .WithStartOnSiteMilestone(new StartOnSiteMilestoneDetailsBuilder().WithStartOnSiteDate(today.AddDays(2))
                .WithPaymentDate(today.AddDays(3))
                .Build())
            .WithCompletionMilestone(new CompletionMilestoneDetailsBuilder().WithCompletionDate(today.AddDays(4))
                .WithPaymentDate(today.AddDays(5))
                .Build())
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.acquisitionDate.Should().Be(new DateTime(2024, 01, 28, 0, 0, 0, DateTimeKind.Unspecified));
        result.acquisitionPaymentDate.Should().Be(new DateTime(2024, 01, 29, 0, 0, 0, DateTimeKind.Unspecified));
        result.startOnSiteDate.Should().Be(new DateTime(2024, 01, 30, 0, 0, 0, DateTimeKind.Unspecified));
        result.startOnSitePaymentDate.Should().Be(new DateTime(2024, 01, 31, 0, 0, 0, DateTimeKind.Unspecified));
        result.completionDate.Should().Be(new DateTime(2024, 02, 01, 0, 0, 0, DateTimeKind.Unspecified));
        result.completionPaymentDate.Should().Be(new DateTime(2024, 02, 02, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(true, "yes")]
    [InlineData(false, "no")]
    public void ShouldMapAdditionalPaymentRequested(bool? requiresAdditionalPayments, string? expectedResult)
    {
        // given
        var entity = requiresAdditionalPayments.HasValue
            ? new DeliveryPhaseEntityBuilder().WithAdditionalPaymentRequested(new IsAdditionalPaymentRequested(requiresAdditionalPayments.Value)).Build()
            : new DeliveryPhaseEntityBuilder().Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.requiresAdditionalPayments.Should().Be(expectedResult);
    }
}
