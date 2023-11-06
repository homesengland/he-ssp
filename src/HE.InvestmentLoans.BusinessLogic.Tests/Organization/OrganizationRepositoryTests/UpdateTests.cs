extern alias Org;

using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;
using OrganizationDetailsDto = Org::HE.Common.IntegrationModel.PortalIntegrationModel.OrganizationDetailsDto;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.OrganizationRepositoryTests;

public class UpdateTests : TestBase<OrganizationRepository>
{
    [Fact]
    public async Task ShouldSendOrganisationChangeDetailsRequest()
    {
        // Given
        var organisationEntity = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .OrganizationEntityMock;

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var organizationServiceMockTestBuilder = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .BuildMock();

        // when
        await TestCandidate.Update(organisationEntity, userAccount, CancellationToken.None);

        // then
        organizationServiceMockTestBuilder.Verify(
            x => x.CreateOrganisationChangeRequest(
                It.IsAny<OrganizationDetailsDto>(), userAccount.UserGlobalId.ToString()),
            Times.Once);
    }
}
