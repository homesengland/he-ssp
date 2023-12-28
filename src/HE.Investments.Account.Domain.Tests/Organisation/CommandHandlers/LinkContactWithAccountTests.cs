extern alias Org;

using FluentAssertions;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Domain.Organisation.CommandHandlers;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Account.Domain.Tests.User.TestObjectBuilder;
using HE.Investments.Common.Errors;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Tests.Extensions.FluentAssertionsExtensions;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;
using static FluentAssertions.FluentActions;
using IContactService = Org::HE.Investments.Organisation.Services.IContactService;
using OrganisationSearchItem = Org::HE.Investments.Organisation.Contract.OrganisationSearchItem;
using OrganizationDetailsDto = Org::HE.Common.IntegrationModel.PortalIntegrationModel.OrganizationDetailsDto;

namespace HE.Investments.Account.Domain.Tests.Organisation.CommandHandlers;

public class LinkContactWithAccountTests : TestBase<LinkContactWithOrganizationCommandHandler>
{
    private readonly LinkContactWithOrganizationCommand _command;

    public LinkContactWithAccountTests()
    {
        _command = new LinkContactWithOrganizationCommand("12345");

        RegisterDependency(new Mock<IContactService>());
    }

    [Fact]
    public async Task FailWhenAccountIsAlreadyLinkedWithOrganization()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsLinkedWithOrganization()
            .Register(this);

        // when
        // then
        await Invoking(() => TestCandidate.Handle(_command, CancellationToken.None))
            .Should()
            .ThrowAsync<DomainException>()
            .WithErrorCode(CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
    }

    [Fact]
    public async Task FailWhenSearchServiceRequestWasUnsuccessful()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .ReturnUnsuccessfulResponse()
            .Register(this);

        // when
        // then
        await Invoking(() => TestCandidate.Handle(_command, CancellationToken.None)).Should().ThrowAsync<ExternalServiceException>();
    }

    [Fact]
    public async Task FailWhenOrganizationCannotBeFound()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .GeOrganisationReturnsNoOrganization()
            .Register(this);

        // when
        // then
        await Invoking(() => TestCandidate.Handle(_command, CancellationToken.None)).Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateOrganizationInCrm_WhenLinkedOrganizationDoesntExistInCrm()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .Returns(OrganizationThatDoesNotExistInCrm())
            .Register(this);

        var organizationService = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .BuildMock();

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        organizationService.Verify(c => c.CreateOrganization(It.IsAny<OrganizationDetailsDto>()), Times.Once);
    }

    [Fact]
    public async Task DoNotCreateOrganizationInCrm_WhenLinkedOrganizationExistsInCrm()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .Returns(OrganizationThatExistsInCrm())
            .Register(this);

        var organizationService = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .BuildMock();

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        organizationService.Verify(c => c.CreateOrganization(It.IsAny<OrganizationDetailsDto>()), Times.Never);
    }

    private OrganisationSearchItem OrganizationThatDoesNotExistInCrm()
    {
        return new OrganisationSearchItem("12345", "anyName", "anyCity", "Any Street 1", "ABCD 123", null, false);
    }

    private OrganisationSearchItem OrganizationThatExistsInCrm()
    {
        return new OrganisationSearchItem("12345", "anyName", "anyCity", "Any Street 1", "ABCD 123", GuidTestData.GuidTwo.ToString(), true);
    }
}
