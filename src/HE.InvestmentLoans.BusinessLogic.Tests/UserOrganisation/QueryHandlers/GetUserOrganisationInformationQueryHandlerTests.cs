using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.UserOrganisation.QueryHandlers;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.UserOrganisation.QueryHandlers;

public class GetUserOrganisationInformationQueryHandlerTests : TestBase<GetUserOrganisationInformationQueryHandler>
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldReturnUserOrganisationInformation(bool isLimitedUser)
    {
        // given
        var userAccount = LoanUserContextTestBuilder
            .New(isLimitedUser ? null : UserAccountTestData.AdminUserAccountOne)
            .Register(this)
            .UserAccountFromMock;

        var userDetails = LoanUserContextTestBuilder
            .New(isLimitedUser ? null : UserAccountTestData.AdminUserAccountOne)
            .Register(this)
            .UserDetailsFromMock;

        UserRepositoryTestBuilder
            .New()
            .ReturnUserDetailsEntity(userAccount.UserGlobalId, userDetails)
            .BuildMockAndRegister(this);

        var organisation = OrganizationBasicInformationTestBuilder.New().Build();

        CreateAndRegisterDependencyMock<IOrganizationRepository>()
            .Setup(i => i.GetBasicInformation(userAccount, It.IsAny<CancellationToken>()))
            .ReturnsAsync(organisation);

        var applications = new List<UserLoanApplication>();
        CreateAndRegisterDependencyMock<ILoanApplicationRepository>()
            .Setup(i => i.LoadAllLoanApplications(userAccount, It.IsAny<CancellationToken>()))
            .ReturnsAsync(applications);

        // when
        var result = await TestCandidate.Handle(new GetUserOrganisationInformationQuery(), CancellationToken.None);

        // then
        result.OrganizationBasicInformation.Should().Be(organisation);
        result.LoanApplications.Should().BeEquivalentTo(applications);
        result.IsLimitedUser.Should().Be(isLimitedUser);
        result.UserFirstName.Should().Be(userDetails.FirstName);
    }
}
