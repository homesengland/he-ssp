using FluentAssertions;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Tests.Organisation.TestData;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.Common.User;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;
using UserAccount = HE.Investments.Account.Shared.User.UserAccount;

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
        var fakeUserAccount = new UserAccount(
            UserGlobalId.From("FakeId"),
            string.Empty,
            new OrganisationBasicInfo(
                new OrganisationId(GuidTestData.GuidTwo.ToString()),
                "AccountTwo",
                "4321",
                "Main street",
                "London",
                "Postal code",
                false),
            []);

        OrganizationServiceMockTestBuilder
            .New()
            .ReturnUserAccount(userAccount)
            .ReturnOrganizationDetailsDto(organizationDetailsDto)
            .Register(this);
        RegisterUserContext(fakeUserAccount.UserGlobalId.Value);

        // when
        var action = () => TestCandidate.GetBasicInformation(fakeUserAccount.SelectedOrganisationId(), CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"*{fakeUserAccount.Organisation!.OrganisationId}*");
    }

    private void RegisterUserContext(string userGlobalId)
    {
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(x => x.UserGlobalId).Returns(userGlobalId);

        RegisterDependency(userContextMock.Object);
    }
}
