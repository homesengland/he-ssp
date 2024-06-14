using FluentAssertions;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Domain.Organisation.CommandHandlers;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.Organisation.Services;
using HE.Investments.Organisation.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using Xunit;
using static FluentAssertions.FluentActions;

namespace HE.Investments.Account.Domain.Tests.Organisation.CommandHandlers;

public class LinkContactWithAccountTests : TestBase<LinkContactWithOrganizationCommandHandler>
{
    [Fact]
    public async Task ShouldThrowException_WhenAccountIsAlreadyLinkedWithOrganization()
    {
        // given
        IList<UserAccount> userAccounts = [UserAccountTestData.UserAccountOne];

        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .ReturnUserAccounts(userAccounts)
            .Register(this);

        var organisationServiceMock = new Mock<IInvestmentsOrganisationService>();
        var contactServiceMock = new Mock<IContactService>();

        organisationServiceMock.Setup(x => x.GetOrganisation(new OrganisationIdentifier(userAccounts[0].Organisation!.OrganisationId.ToString()), CancellationToken.None))
            .ReturnsAsync(new InvestmentsOrganisation(userAccounts[0].Organisation!.OrganisationId, userAccounts[0].Organisation!.RegisteredCompanyName));

        RegisterDependency(organisationServiceMock.Object);
        RegisterDependency(contactServiceMock.Object);

        // when && then
        await Invoking(() => TestCandidate.Handle(new LinkContactWithOrganisationCommand(userAccounts[0].Organisation!.OrganisationId.ToString(), true), CancellationToken.None))
            .Should()
            .ThrowAsync<DomainValidationException>()
            .Where(x => x.Message == $"You are already linked with {userAccounts[0].Organisation!.RegisteredCompanyName}");
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
        await TestCandidate.Handle(new LinkContactWithOrganisationCommand("12345", true), CancellationToken.None);

        // then
        contactServiceMock.Verify(x => x.LinkContactWithOrganization(It.IsAny<IOrganizationServiceAsync2>(), "UserOne", organisationId.ToString(), 858110002), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowException_WhenIsConfirmedIsNotProvided()
    {
        // given
        AccountUserContextTestBuilder
            .New()
            .IsNotLinkedWithOrganization()
            .Register(this);

        // when && then
        await Invoking(() => TestCandidate.Handle(new LinkContactWithOrganisationCommand("12345", null), CancellationToken.None))
            .Should()
            .ThrowAsync<DomainValidationException>()
            .Where(x => x.Message == ValidationErrorMessage.ChooseYourAnswer);
    }
}
