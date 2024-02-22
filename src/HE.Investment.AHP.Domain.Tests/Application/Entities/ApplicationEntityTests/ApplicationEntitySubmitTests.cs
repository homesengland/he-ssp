using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investment.AHP.Domain.Application.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Application.TestData;
using HE.Investment.AHP.Domain.Tests.Application.TestObjectBuilder;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using ApplicationSection = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationSection;

namespace HE.Investment.AHP.Domain.Tests.Application.Entities.ApplicationEntityTests;

public class ApplicationEntitySubmitTests : TestBase<ApplicationEntity>
{
    [Fact]
    public async Task ShouldSubmitApplication()
    {
        // given
        var application = ApplicationEntityBuilder
            .New()
            .WithSections(new ApplicationSection(SectionType.Scheme, SectionStatus.Completed), new ApplicationSection(SectionType.DeliveryPhases, SectionStatus.Completed))
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
        await application.Submit(applicationRepository.Object, organisationId, new RepresentationsAndWarranties("checked"), CancellationToken.None);

        // then
        applicationRepository.Verify(repo => repo.ChangeApplicationStatus(application, organisationId, null, CancellationToken.None), Times.Once);
        application.IsModified.Should().BeTrue();
        application.Status.Should().Be(ApplicationStatus.ApplicationSubmitted);
    }

    [Fact]
    public async Task ShouldThrowException_WhenSectionsNotCompleted()
    {
        // given
        var application = ApplicationEntityBuilder
            .New()
            .WithSections(new ApplicationSection(SectionType.Scheme, SectionStatus.InProgress), new ApplicationSection(SectionType.DeliveryPhases, SectionStatus.Completed))
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
        var result = async () => await application.Submit(applicationRepository.Object, organisationId, new RepresentationsAndWarranties("checked"), CancellationToken.None);

        // then
        await result.Should().ThrowAsync<DomainValidationException>();
        application.IsModified.Should().BeFalse();
        application.Status.Should().Be(ApplicationStatus.New);
    }
}
