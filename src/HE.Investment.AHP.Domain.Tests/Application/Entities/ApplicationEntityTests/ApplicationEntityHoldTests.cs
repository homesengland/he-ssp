using FluentAssertions;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.Application.TestObjectBuilder;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntityHoldTests : TestBase<ApplicationEntity>
{
    [Fact]
    public async Task ShouldChangeApplicationStatusToHold_WhenCurrentStatusIsDraft()
    {
        var holdReason = HoldReasonTestData.HoldReasonOne;

        var application = ApplicationEntityBuilder
            .New()
            .WithApplicationStatus(ApplicationStatus.Draft)
            .Build();

        var applicationId = AhpApplicationIdTestData.AhpApplicationIdOne;
        var organisationId = OrganisationIdTestData.OrganisationIdOne;

        var userAccount = AccountUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var applicationRepository = ApplicationRepositoryTestBuilder
            .New()
            .ReturnApplication(applicationId, userAccount, application)
            .BuildIChangeApplicationStatusMockAndRegister(this);

        // when
        await application.Hold(applicationRepository.Object, holdReason, organisationId, CancellationToken.None);

        // then
        applicationRepository.Verify(repo => repo.ChangeApplicationStatus(application, organisationId, holdReason.Value, CancellationToken.None), Times.Once);
        application.IsModified.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.OnHold);
    }
}
