using FluentAssertions;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.QueryHandlers;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Common.Tests.TestObjectBuilders;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.QueryHandlers;

public class GetOrganizationBasicInformationQueryHandlerTests : TestBase<GetOrganizationBasicInformationQueryHandler>
{
    [Fact]
    public async Task ShouldReturnOrganizationBasicInformation()
    {
        // given
        var userAccount = AccountUserContextTestBuilder
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
