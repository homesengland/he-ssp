extern alias Org;

using FluentAssertions;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Domain.Organisation.CommandHandlers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Errors;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using Org::HE.Investments.Organisation.Services;
using Org::HE.Investments.Organisation.ValueObjects;
using Xunit;
using static FluentAssertions.FluentActions;
using IContactService = Org::HE.Investments.Organisation.Services.IContactService;

namespace HE.Investments.Account.Domain.Tests.Organisation.CommandHandlers;

public class LinkContactWithAccountTests : TestBase<LinkContactWithOrganizationCommandHandler>
{
    private readonly LinkContactWithOrganisationCommand _command;

    public LinkContactWithAccountTests()
    {
        _command = new LinkContactWithOrganisationCommand("12345");

        RegisterDependency(new Mock<IContactService>());
    }

    [Fact]
    public async Task ShouldThrowException_WhenAccountIsAlreadyLinkedWithOrganization()
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
            .Where(x => x.ErrorCode == CommonErrorCodes.ContactAlreadyLinkedWithOrganization);
    }

    [Fact]
    public async Task ShouldLinkContactWithOrganisation_WhenAccountIsNotLinkedWithOrganisation()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        var organisationId = Guid.NewGuid();
        var organisationServiceMock = new Mock<IInvestmentsOrganisationService>();
        var contactServiceMock = new Mock<IContactService>();

        organisationServiceMock.Setup(x => x.GetOrganisation(new OrganisationIdentifier("12345"), CancellationToken.None))
            .ReturnsAsync(new InvestmentsOrganisation(new OrganisationId(organisationId.ToString()), "anyName"));

        RegisterDependency(organisationServiceMock.Object);
        RegisterDependency(contactServiceMock.Object);

        // when
        await TestCandidate.Handle(_command, CancellationToken.None);

        // then
        contactServiceMock.Verify(x => x.LinkContactWithOrganization(It.IsAny<IOrganizationServiceAsync2>(), "UserOne", organisationId, 858110002), Times.Once);
    }
}
