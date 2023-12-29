extern alias Org;

using FluentAssertions;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Tests.Organisation.TestData;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Account.Domain.Tests.User.TestData;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.User;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.OrganizationRepositoryTests;

public class GetBasicInformationTests : TestBase<OrganizationRepository>
{
    [Fact]
    public async Task ShouldReturnOrganizationBasicInformation_WhenUserGlobalIdAndAccountIdAreCorrect()
    {
        // given
        var organizationDetailsDto = OrganizationDetailsDtoTestData.OrganizationDetailsDto;
        var userAccount = UserAccountTestData.UserAccountOne;

        OrganizationServiceMockTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .ReturnOrganizationDetailsDto(organizationDetailsDto)
            .Register(this);
        RegisterUserContext(userAccount.UserGlobalId.Value);

        // when
        var result = await TestCandidate.GetBasicInformation(userAccount.SelectedOrganisationId(), CancellationToken.None);

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
        var organizationDetailsDto = OrganizationDetailsDtoTestData.OrganizationDetailsDto;
        var userAccount = UserAccountTestData.UserAccountOne;
        var fakeUserAccount = new UserAccount(UserGlobalId.From("FakeId"), string.Empty, new OrganisationId(GuidTestData.GuidTwo), string.Empty, Array.Empty<UserRole>());

        OrganizationServiceMockTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .ReturnOrganizationDetailsDto(organizationDetailsDto)
            .Register(this);
        RegisterUserContext(fakeUserAccount.UserGlobalId.Value);

        // when
        var action = () => TestCandidate.GetBasicInformation(fakeUserAccount.SelectedOrganisationId(), CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{fakeUserAccount.OrganisationId}*");
    }

    private void RegisterUserContext(string userGlobalId)
    {
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(x => x.UserGlobalId).Returns(userGlobalId);

        RegisterDependency(userContextMock.Object);
    }
}
