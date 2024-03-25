extern alias Org;

using FluentAssertions;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Domain.Tests.UserOrganisation.TestDataBuilders;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.Repositories.OrganisationUsersRepositoryTests;

public class SaveTests : TestBase<OrganisationUsersRepository>
{
    [Fact]
    public async Task ShouldCreateNotConnectedContactAndMarkInvitationAsSent()
    {
        // given
        var invitation = new UserInvitationEntityTestDataBuilder()
            .WithRole(UserRole.Enhanced)
            .WithEmailAddress("myemail@he.gov.uk")
            .Build();
        var organisationUsers = new OrganisationUsersEntityTestDataBuilder().Build();
        var contactService = CreateAndRegisterDependencyMock<IContactService>();
        var userContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        userContext.Setup(x => x.UserGlobalId).Returns(UserGlobalId.From("admin-user-id"));

        // when
        organisationUsers.InviteUser(invitation);
        await TestCandidate.Save(organisationUsers, CancellationToken.None);

        // then
        organisationUsers.PendingInvitations.Should().BeEmpty();
        organisationUsers.InvitedUsers.Should().HaveCount(1).And.Contain(new EmailAddress("myemail@he.gov.uk"));

        contactService.Verify(
            x => x.CreateNotConnectedContact(
                It.IsAny<IOrganizationServiceAsync2>(),
                It.Is<ContactDto>(y => y.email == "myemail@he.gov.uk"),
                organisationUsers.OrganisationId.Value,
                858110004,
                "admin-user-id",
                null),
            Times.Once);
    }

    [Fact]
    public async Task ShouldDispatchDomainEvent()
    {
        // given
        var organisationUsers = new OrganisationUsersEntityTestDataBuilder().Build();
        var eventDispatcher = CreateAndRegisterDependencyMock<IEventDispatcher>();

        // when
        await TestCandidate.Save(organisationUsers, CancellationToken.None);

        // then
        eventDispatcher.Verify(x => x.Publish(organisationUsers, CancellationToken.None), Times.Once);
    }
}
