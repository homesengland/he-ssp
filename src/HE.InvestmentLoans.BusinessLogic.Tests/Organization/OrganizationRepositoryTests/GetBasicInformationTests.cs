using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.Tests.User.TestObjectBuilder;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Common.Tests.TestFramework;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using Microsoft.Crm.Sdk.Messages;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.OrganizationRepositoryTests;
public class GetBasicInformationTests : TestBase<OrganizationRepository>
{
    [Fact]
    public async Task ShouldReturnOrganizationBasicInformation_WhenUserGlobalIdAndAccountIdAreCorrect()
    {
        // given
        var organizationDetailsDto = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .OrganizationDetailsDtoMock;

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        OrganizationServiceMockTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .ReturnOrganizationDetailsDto(organizationDetailsDto)
            .Register(this);

        // when
        var result = await TestCandidate.GetBasicInformation(userAccount, CancellationToken.None);

        // then
        result.CompanyRegistrationNumber.Should().Be(organizationDetailsDto.companyRegistrationNumber);
        result.RegisteredCompanyName.Should().Be(organizationDetailsDto.registeredCompanyName);
        result.Address.Line1.Should().Be(organizationDetailsDto.addressLine1);
        result.Address.Line2.Should().Be(organizationDetailsDto.addressLine2);
        result.Address.Line3.Should().Be(organizationDetailsDto.addressLine3);
        result.Address.City.Should().Be(organizationDetailsDto.city);
        result.Address.Country.Should().Be(organizationDetailsDto.country);
        result.Address.PostalCode.Should().Be(organizationDetailsDto.postalcode);
    }

    [Fact]
    public async Task ShouldThrowNotFoundException_WhenUserGlobalIdAndAccountIdAreNotCorrect()
    {
        // given
        var organizationDetailsDto = OrganizationServiceMockTestBuilder
            .New()
            .Register(this)
            .OrganizationDetailsDtoMock;

        var userAccount = LoanUserContextTestBuilder
            .New()
            .Register(this)
            .UserAccountFromMock;

        var fakeUserAccount = new UserAccount(UserGlobalId.From("FakeId"), string.Empty, GuidTestData.GuidTwo, string.Empty, Array.Empty<UserAccountRole>());

        OrganizationServiceMockTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .ReturnOrganizationDetailsDto(organizationDetailsDto)
            .Register(this);

        // when
        var action = () => TestCandidate.GetBasicInformation(fakeUserAccount, CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{fakeUserAccount.AccountId}*");
    }
}
