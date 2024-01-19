using FluentAssertions;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.Application.TestObjectBuilder;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntityWithdrawTests : TestBase<ApplicationEntity>
{
    [Fact]
    public async Task ShouldChangeApplicationStatusToWithdraw_WhenCurrentStatusIsApplicationSubmitted()
    {
        var withdrawReason = WithdrawReasonTestData.WithdrawReasonOne;

        var application = ApplicationEntityBuilder
            .New()
            .WithApplicationStatus(ApplicationStatus.ApplicationSubmitted)
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
            .BuildIApplicationWithdrawMockAndRegister(this);

        // when
        await application.Withdraw(applicationRepository.Object, withdrawReason, organisationId, CancellationToken.None);

        // then
        applicationRepository.Verify(repo => repo.Withdraw(application, organisationId, CancellationToken.None), Times.Once);
        application.IsModified.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.Withdrawn);
    }
}
