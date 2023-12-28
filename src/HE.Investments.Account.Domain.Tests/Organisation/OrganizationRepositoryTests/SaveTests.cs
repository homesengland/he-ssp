extern alias Org;

using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Tests.Organisation.TestData;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Account.Domain.Tests.User.TestData;
using HE.Investments.Common.User;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.OrganizationRepositoryTests;

public class SaveTests : TestBase<OrganizationRepository>
{
    [Fact]
    public async Task ShouldSendOrganisationChangeDetailsRequest()
    {
        // Given
        var organisationEntity = OrganisationEntityTestData.OrganisationEntity;
        var userAccount = UserAccountTestData.UserAccountOne;
        var organizationServiceMockTestBuilder = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .BuildMock();
        RegisterUserContext(userAccount.UserGlobalId.Value);

        // when
        await TestCandidate.Save(userAccount.SelectedOrganisationId(), organisationEntity, CancellationToken.None);

        // then
        organizationServiceMockTestBuilder.Verify(
            x => x.CreateOrganisationChangeRequest(
                It.IsAny<OrganizationDetailsDto>(), userAccount.UserGlobalId.ToString()),
            Times.Once);
    }

    private void RegisterUserContext(string userGlobalId)
    {
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(x => x.UserGlobalId).Returns(userGlobalId);

        RegisterDependency(userContextMock.Object);
    }
}
