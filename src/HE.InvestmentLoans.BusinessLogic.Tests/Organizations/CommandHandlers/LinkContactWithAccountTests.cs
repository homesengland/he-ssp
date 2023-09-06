extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Organizations.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Moq;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organizations.CommandHandlers;

public class LinkContactWithAccountTests : TestBase<IRequestHandler<LinkContactWithOrganizationCommand>>
{
    private readonly LinkContactWithOrganizationCommand _command;

    public LinkContactWithAccountTests()
    {
        _command = new LinkContactWithOrganizationCommand(new CompaniesHouseNumber("12345"));

        RegisterDependency(new Mock<IContactService>());

        RegisterInterfaceImplementation<IRequestHandler<LinkContactWithOrganizationCommand>, LinkContactWithOrganizationCommandHandler>();
    }

    [Fact]
    public async Task FailWhenAccountIsAlreadyLinkedWithOrganization()
    {
        // given
        LoanUserContextTestBuilder
            .New()
            .IsLinkedWithOrganization()
            .Register(this);

        // when
        Func<Task> testAction = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await testAction.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task FailWhenSearchServiceRequestWasUnsuccessful()
    {
        // given
        LoanUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .ReturnUnsuccessfulResponse()
            .Register(this);

        // when
        Func<Task> testAction = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await testAction.Should().ThrowAsync<ExternalServiceException>();
    }

    [Fact]
    public async Task FailWhenOrganizationCannotBeFound()
    {
        // given
        LoanUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .ReturnsNoOrganization()
            .Register(this);

        // when
        Func<Task> testAction = () => TestCandidate.Handle(_command, CancellationToken.None);

        // then
        await testAction.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateOrganizationInCrm_WhenLinkedOrganizationDoesntExistInCrm()
    {
        // given
        LoanUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .Returns(OrganizationThatDoesNotExistInCrm())
            .Register(this);

        var organizationService = OrganizationServiceTestBuilder
            .New()
            .Register(this)
            .Mock();

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        organizationService.Verify(c => c.CreateOrganization(It.IsAny<OrganizationDetailsDto>()), Times.Once);
    }

    [Fact]
    public async Task DoNotCreateOrganizationInCrm_WhenLinkedOrganizationExistsInCrm()
    {
        // given
        LoanUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        OrganizationSearchServiceTestBuilder
            .New()
            .Returns(OrganizationThatExistsInCrm())
            .Register(this);

        var organizationService = OrganizationServiceTestBuilder
            .New()
            .Register(this)
            .Mock();

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        organizationService.Verify(c => c.CreateOrganization(It.IsAny<OrganizationDetailsDto>()), Times.Never);
    }

    private OrganisationSearchItem OrganizationThatDoesNotExistInCrm()
    {
        return new OrganisationSearchItem("12345", "anyName", "anyCity", "Any Street 1", "ABCD 123", false);
    }

    private OrganisationSearchItem OrganizationThatExistsInCrm()
    {
        return new OrganisationSearchItem("12345", "anyName", "anyCity", "Any Street 1", "ABCD 123", true);
    }
}
