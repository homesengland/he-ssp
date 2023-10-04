extern alias Org;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.UserRepositoryTests;
public class SaveAsyncTests : TestBase<LoanUserRepository>
{
    [Fact]
    public async Task ShouldSaveUserDetailsEntityWithAllChanges()
    {
        // Given
        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var contactServiceMock = ContactServiceMockTestBuilder
            .New()
            .Register(this)
            .BuildMock();

        var newUserDetails = new UserDetails("John", "Smith", "Developer", "john.smith@test.com", "12345678", "87654321", false);

        // when
        await TestCandidate.SaveAsync(newUserDetails, userAccount.UserGlobalId, CancellationToken.None);

        // then
        contactServiceMock.Verify(
            x => x.UpdateUserProfile(
                It.IsAny<IOrganizationServiceAsync2>(),
                userAccount.UserGlobalId.ToString(),
                It.IsAny<ContactDto>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
