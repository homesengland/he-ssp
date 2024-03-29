using HE.Investments.Common.CRM.Model;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Moq;
using Xunit;

namespace HE.Investments.Loans.BusinessLogic.Tests.LoanApplication.LoanApplicationRepositoryTests;

public class IsExistTests : TestBase<LoanApplicationRepository>
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldReturnValueMappedFromCrmResponse(bool isExist)
    {
        // given
        var loanApplicationName = LoanApplicationNameTestData.MyFirstApplication;
        var userAccount = UserAccountTestData.UserAccountOne;

        RegisterCrmMock(loanApplicationName.Value, userAccount.SelectedOrganisationId().ToString(), isExist);

        // when
        var result = await TestCandidate.IsExist(loanApplicationName, userAccount, CancellationToken.None);

        // then
        result.Should().Be(isExist);
    }

    private void RegisterCrmMock(string loanApplicationName, string organisationId, bool isExist)
    {
        var responseResults = new ParameterCollection();
        responseResults.AddOrUpdateIfNotNull("invln_loanexists", isExist);

        CreateAndRegisterDependencyMock<IOrganizationServiceAsync2>()
            .Setup(
                x => x.ExecuteAsync(
                    It.Is<invln_checkifloanapplicationwithgivennameexistsRequest>(y =>
                        y.invln_loanname == loanApplicationName && y.invln_organisationid == organisationId),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(new invln_checkifloanapplicationwithgivennameexistsResponse { Results = responseResults });
    }
}
