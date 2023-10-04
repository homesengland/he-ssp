using HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.QueryHandlers;

public class GetOrganizationByCompanyHouseNumberQueryHandlerTests : TestBase<GetOrganizationByCompanyHouseNumberQueryHandler>
{
    [Fact]
    public async Task ShouldReturnBasicData()
    {
        // given
        const string companyHouseNumber = "12345678";

        OrganizationSearchServiceTestBuilder.New().ReturnsOneOrganization(companyHouseNumber, out var searchItem).Register(this);

        // when
        var result = await TestCandidate.Handle(new GetOrganizationByCompanyHouseNumberQuery(companyHouseNumber), CancellationToken.None);

        // then
        result.CompaniesHouseNumber.Should().Be(companyHouseNumber);
        result.Name.Should().Be(searchItem.Name);
        result.City.Should().Be(searchItem.City);
        result.Street.Should().Be(searchItem.Street);
        result.PostalCode.Should().Be(searchItem.PostalCode);
    }

    [Fact]
    public async Task ShouldThrowExceptionNotFound_WhenOrganizationWithGivenNumberDoesNotExist()
    {
        // given
        const string notExistingCompanyHouseNumber = "12345678";

        OrganizationSearchServiceTestBuilder
            .New()
            .ReturnsNoOrganization()
            .Register(this);

        // when
        var action = async () =>
            await TestCandidate.Handle(new GetOrganizationByCompanyHouseNumberQuery(notExistingCompanyHouseNumber), CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"Organization with company house number {notExistingCompanyHouseNumber} not found");
    }
}
