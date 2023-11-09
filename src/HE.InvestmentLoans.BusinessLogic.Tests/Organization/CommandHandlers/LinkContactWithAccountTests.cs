extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.Extensions.FluentAssertionsExtensions;
using HE.InvestmentLoans.Contract;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;
using Xunit;
using static FluentAssertions.FluentActions;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.CommandHandlers;

public class LinkContactWithAccountTests : TestBase<LinkContactWithOrganizationCommandHandler>
{
    private readonly LinkContactWithOrganizationCommand _command;

    public LinkContactWithAccountTests()
    {
        _command = new LinkContactWithOrganizationCommand(new CompaniesHouseNumber("12345"));

        RegisterDependency(new Mock<IContactService>());
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
        LoanUserContextTestBuilder
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
        LoanUserContextTestBuilder
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
        LoanUserContextTestBuilder
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
        LoanUserContextTestBuilder
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
        return new OrganisationSearchItem("12345", "anyName", "anyCity", "Any Street 1", "ABCD 123", "IdFromCrm123", true);
    }
}
