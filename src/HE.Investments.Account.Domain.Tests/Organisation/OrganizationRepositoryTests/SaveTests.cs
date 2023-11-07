extern alias Org;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Account.Domain.Tests.User.TestData;
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
        var organisationEntity = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .OrganizationEntityMock;

        var userAccount = UserAccountTestData.UserAccountOne;

        var organizationServiceMockTestBuilder = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .BuildMock();

        // when
        await TestCandidate.Save(organisationEntity, userAccount, CancellationToken.None);

        // then
        organizationServiceMockTestBuilder.Verify(
            x => x.CreateOrganisationChangeRequest(
                It.IsAny<OrganizationDetailsDto>(), userAccount.UserGlobalId.ToString()),
            Times.Once);
    }
}
