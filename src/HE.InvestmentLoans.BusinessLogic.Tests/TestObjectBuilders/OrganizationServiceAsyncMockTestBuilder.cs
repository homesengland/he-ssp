using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.CRM.Model;
using Microsoft.PowerPlatform.Dataverse.Client;
using Moq;

namespace HE.InvestmentLoans.BusinessLogic.Tests.TestObjectBuilders;

public class OrganizationServiceAsyncMockTestBuilder
{
    private readonly Mock<IOrganizationServiceAsync2> _mock;

    private OrganizationServiceAsyncMockTestBuilder()
    {
        _mock = new Mock<IOrganizationServiceAsync2>();
    }

    public static OrganizationServiceAsyncMockTestBuilder New() => new();

    public OrganizationServiceAsyncMockTestBuilder MockGetSingleLoanApplicationForAccountAndContactRequest(
        LoanApplicationId loanApplicationId,
        string rawResponse)
    {
        var response = new invln_getsingleloanapplicationforaccountandcontactResponse();
        response.Results["invln_loanapplication"] = rawResponse;

        _mock
            .Setup(x => x.ExecuteAsync(
                It.Is<invln_getsingleloanapplicationforaccountandcontactRequest>(y => y.invln_loanapplicationid == loanApplicationId.ToString()),
                CancellationToken.None))
            .ReturnsAsync(response);

        return this;
    }

    public Mock<IOrganizationServiceAsync2> BuildMock()
    {
        return _mock;
    }

    public IOrganizationServiceAsync2 Build()
    {
        return _mock.Object;
    }
}
