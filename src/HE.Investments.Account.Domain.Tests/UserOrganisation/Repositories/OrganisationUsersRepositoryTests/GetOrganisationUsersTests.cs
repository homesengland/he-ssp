extern alias Org;

using FluentAssertions;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.UserOrganisation.Repositories.OrganisationUsersRepositoryTests;

public class GetOrganisationUsersTests : TestBase<OrganisationUsersRepository>
{
    private static readonly OrganisationId OrganisationId = new(Guid.NewGuid());

    [Fact]
    public async Task ShouldReturnUserAsActive_WhenIsConnectedWithExternalIdentity()
    {
        // given
        MockContacts(
            new ContactDto { email = "active1@user.com", contactExternalId = "externalUserId" },
            new ContactDto { email = "notconnected1@user.com", contactExternalId = "_notconnectedcontact" },
            new ContactDto { email = "invited2@user.com", contactExternalId = string.Empty },
            new ContactDto { email = "invited3@user.com", contactExternalId = null });

        // when
        var result = await TestCandidate.GetOrganisationUsers(OrganisationId, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.OrganisationId.Should().Be(OrganisationId);
        result.ActiveUsers.Should().HaveCount(1).And.Contain(new EmailAddress("active1@user.com"));
    }

    [Fact]
    public async Task ShouldReturnUserAsInvited_WhenIsNotConnectedWithExternalIdentity()
    {
        // given
        MockContacts(
            new ContactDto { email = "active1@user.com", contactExternalId = "externalUserId" },
            new ContactDto { email = "notconnected1@user.com", contactExternalId = "_notconnectedcontact" },
            new ContactDto { email = "invited2@user.com", contactExternalId = string.Empty },
            new ContactDto { email = "invited3@user.com", contactExternalId = null });

        // when
        var result = await TestCandidate.GetOrganisationUsers(OrganisationId, CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.OrganisationId.Should().Be(OrganisationId);
        result.InvitedUsers.Should()
            .HaveCount(3)
            .And.Contain(new EmailAddress("notconnected1@user.com"))
            .And.Contain(new EmailAddress("invited2@user.com"))
            .And.Contain(new EmailAddress("invited3@user.com"));
    }

    private void MockContacts(params ContactDto[] contacts)
    {
        var contactService = CreateAndRegisterDependencyMock<IContactService>();

        contactService.Setup(x => x.GetAllOrganisationContactsForPortal(
                It.IsAny<IOrganizationServiceAsync2>(),
                OrganisationId.Value,
                null))
            .ReturnsAsync(contacts.ToList());
    }
}
