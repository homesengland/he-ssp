using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.CompanyStructure.TestObjectBuilders;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.User.Queries;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.QueryHandlers;
public class GetOrganizationBasicInformationQueryHandlerTests : TestBase<GetOrganizationBasicInformationQueryHandler>
{
    [Fact]
    public async Task ShouldReturnOrganizationBasicInformation()
    {
        // given
        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var organizationBasicInformation = OrganizationBasicInformationTestBuilder
            .New()
            .Build();

        OrganizationRepositoryTestBuilder
            .New()
            .ReturnOrganizationBasicInformationEntity(userAccount, organizationBasicInformation)
            .BuildMockAndRegister(this);

        // when
        var result = await TestCandidate.Handle(new GetOrganizationBasicInformationQuery(), CancellationToken.None);

        // then
        result.OrganizationBasicInformation.RegisteredCompanyName.Should().Be(organizationBasicInformation.RegisteredCompanyName);
        result.OrganizationBasicInformation.CompanyRegistrationNumber.Should().Be(organizationBasicInformation.CompanyRegistrationNumber);
        result.OrganizationBasicInformation.Address.Should().Be(organizationBasicInformation.Address);
    }
}
