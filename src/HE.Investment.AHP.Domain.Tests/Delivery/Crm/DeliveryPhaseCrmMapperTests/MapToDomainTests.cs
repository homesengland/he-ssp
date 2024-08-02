using FluentAssertions;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Crm;
using HE.Investment.AHP.Domain.Delivery.Policies;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Crm.DeliveryPhaseCrmMapperTests;

public class MapToDomainTests : TestBase<DeliveryPhaseCrmMapper>
{
    private static readonly ApplicationBasicInfo Application = ApplicationBasicInfoTestData.AffordableRentInDraftState;

    private static readonly OrganisationBasicInfo Organisation = new OrganisationBasicInfoBuilder().Build();

    [Fact]
    public void ShouldMapDeliveryPhaseBasicProperties()
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            createdOn = DateTimeTestData.SeptemberDay20Year2023At0736,
            isReconfigurationOfExistingProperties = true,
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.Id.Should().Be(new DeliveryPhaseId("dp-id-1"));
        result.Name.Should().Be(new DeliveryPhaseName("my name"));
        result.CreatedOn.Should().Be(DateTimeTestData.SeptemberDay20Year2023At0736);
        result.Application.Should().Be(Application);
        result.Organisation.Should().Be(Organisation);
        result.BuildActivity.Should().Be(new BuildActivity(Application.Tenure));
        result.TypeOfHomes.Should().BeNull();
        result.HomesToDeliver.Should().BeEmpty();
        result.ReconfiguringExisting.Should().BeTrue();
        result.DeliveryPhaseMilestones.AcquisitionMilestone.Should().BeNull();
        result.DeliveryPhaseMilestones.StartOnSiteMilestone.Should().BeNull();
        result.DeliveryPhaseMilestones.CompletionMilestone.Should().BeNull();
        result.IsAdditionalPaymentRequested.Should().BeNull();
    }

    [Theory]
    [InlineData(true, SectionStatus.Completed)]
    [InlineData(false, SectionStatus.InProgress)]
    [InlineData(null, SectionStatus.InProgress)]
    public void ShouldMapDeliveryPhaseStatus(bool? isCompleted, SectionStatus expectedStatus)
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            isCompleted = isCompleted,
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.Status.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("newBuild", TypeOfHomes.NewBuild)]
    [InlineData("rehab", TypeOfHomes.Rehab)]
    public void ShouldMapTypeOfHomes(string? typeOfHomes, TypeOfHomes? expectedTypeOfHomes)
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            typeOfHomes = typeOfHomes,
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.TypeOfHomes.Should().Be(expectedTypeOfHomes);
    }

    [Theory]
    [InlineData(858110000, BuildActivityType.AcquisitionAndWorksRehab)]
    [InlineData(858110001, BuildActivityType.ExistingSatisfactory)]
    [InlineData(858110002, BuildActivityType.PurchaseAndRepair)]
    [InlineData(858110003, BuildActivityType.LeaseAndRepair)]
    [InlineData(858110004, BuildActivityType.Reimprovement)]
    [InlineData(858110005, BuildActivityType.Conversion)]
    [InlineData(858110006, BuildActivityType.WorksOnlyRehab)]
    public void ShouldMapBuildActivity_WhenTypeOfHomesIsRehab(int rehabBuildActivityType, BuildActivityType expectedBuildActivityType)
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            typeOfHomes = "rehab",
            rehabBuildActivityType = rehabBuildActivityType,
            newBuildActivityType = null,
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.BuildActivity.Should().NotBeNull();
        result.BuildActivity.Type.Should().Be(expectedBuildActivityType);
    }

    [Theory]
    [InlineData(858110000, BuildActivityType.AcquisitionAndWorks)]
    [InlineData(858110001, BuildActivityType.OffTheShelf)]
    [InlineData(858110002, BuildActivityType.WorksOnly)]
    [InlineData(858110003, BuildActivityType.LandInclusivePackage)]
    [InlineData(858110004, BuildActivityType.Regeneration)]
    public void ShouldMapBuildActivity_WhenTypeOfHomesIsNewBuild(int newBuildActivityType, BuildActivityType expectedBuildActivityType)
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            typeOfHomes = "newBuild",
            rehabBuildActivityType = null,
            newBuildActivityType = newBuildActivityType,
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.BuildActivity.Should().NotBeNull();
        result.BuildActivity.Type.Should().Be(expectedBuildActivityType);
    }

    [Fact]
    public void ShouldMapHomesToDeliver_WhenNumberOfHomesIsProvided()
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            numberOfHomes = new Dictionary<string, int?>
            {
                { "ht-1", null },
                { "ht-2", 0 },
                { "ht-3", 12 },
            },
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy).HomesToDeliver.ToList();

        // then
        result.Should().HaveCount(2);
        result.Single(x => x.HomeTypeId == HomeTypeId.From("ht-2")).Value.Should().Be(0);
        result.Single(x => x.HomeTypeId == HomeTypeId.From("ht-3")).Value.Should().Be(12);
    }

    [Fact]
    public void ShouldMapMilestoneDates_WhenAllAreProvided()
    {
        // given
        var today = new DateTime(2024, 01, 28, 0, 0, 0, DateTimeKind.Unspecified);
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            acquisitionDate = today,
            acquisitionPaymentDate = today.AddDays(1),
            startOnSiteDate = today.AddDays(2),
            startOnSitePaymentDate = today.AddDays(3),
            completionDate = today.AddDays(4),
            completionPaymentDate = today.AddDays(5),
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.DeliveryPhaseMilestones.AcquisitionMilestone.Should().NotBeNull();
        result.DeliveryPhaseMilestones.AcquisitionMilestone!.MilestoneDate!.Value.Should().Be(new DateTime(2024, 01, 28, 0, 0, 0, DateTimeKind.Utc));
        result.DeliveryPhaseMilestones.AcquisitionMilestone!.PaymentDate!.Value.Should().Be(new DateTime(2024, 01, 29, 0, 0, 0, DateTimeKind.Utc));
        result.DeliveryPhaseMilestones.StartOnSiteMilestone!.MilestoneDate!.Value.Should().Be(new DateTime(2024, 01, 30, 0, 0, 0, DateTimeKind.Utc));
        result.DeliveryPhaseMilestones.StartOnSiteMilestone!.PaymentDate!.Value.Should().Be(new DateTime(2024, 01, 31, 0, 0, 0, DateTimeKind.Utc));
        result.DeliveryPhaseMilestones.CompletionMilestone!.MilestoneDate!.Value.Should().Be(new DateTime(2024, 02, 01, 0, 0, 0, DateTimeKind.Utc));
        result.DeliveryPhaseMilestones.CompletionMilestone!.PaymentDate!.Value.Should().Be(new DateTime(2024, 02, 02, 0, 0, 0, DateTimeKind.Utc));
    }

    [Theory]
    [InlineData("yes", true)]
    [InlineData("no", false)]
    public void ShouldMapAdditionalPaymentRequested(string requiresAdditionalPayments, bool expectedResult)
    {
        // given
        var dto = new DeliveryPhaseDto
        {
            id = "dp-id-1",
            name = "my name",
            requiresAdditionalPayments = requiresAdditionalPayments,
        };
        var onlyCompletionMilestonePolicy = WithOnlyCompletionMilestonePolicy();

        // when
        var result = TestCandidate.MapToDomain(Application, Organisation, dto, onlyCompletionMilestonePolicy);

        // then
        result.IsAdditionalPaymentRequested.Should().NotBeNull();
        result.IsAdditionalPaymentRequested!.IsRequested.Should().Be(expectedResult);
    }

    private IOnlyCompletionMilestonePolicy WithOnlyCompletionMilestonePolicy(bool? returnValue = null)
    {
        var onlyCompletionMilestonePolicy = new Mock<IOnlyCompletionMilestonePolicy>();
        onlyCompletionMilestonePolicy
            .Setup(x => x.IsOnlyCompletionMilestone(It.IsAny<bool>(), It.IsAny<BuildActivity>())).Returns(returnValue ?? false);
        return onlyCompletionMilestonePolicy.Object;
    }
}
