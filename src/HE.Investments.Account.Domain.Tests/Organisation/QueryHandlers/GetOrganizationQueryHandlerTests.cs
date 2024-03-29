using FluentAssertions;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.QueryHandlers;
using HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.Account.Domain.Tests.Organisation.QueryHandlers;

public class GetOrganizationQueryHandlerTests : TestBase<GetOrganizationQueryHandler>
{
    [Fact]
    public async Task ShouldReturnBasicData_WhenCompanyHouseNumberIsGiven()
    {
        // given
        const string companyHouseNumber = "12345678";

        OrganizationSearchServiceTestBuilder
            .New()
            .GetByOrganisationReturns(null, companyHouseNumber, out var searchItem)
            .Register(this);

        // when
        var result = await TestCandidate.Handle(new GetOrganisationQuery(companyHouseNumber), CancellationToken.None);

        // then
        result.CompaniesHouseNumber.Should().Be(companyHouseNumber);
        result.Name.Should().Be(searchItem.Name);
        result.City.Should().Be(searchItem.City);
        result.Street.Should().Be(searchItem.Street);
        result.PostalCode.Should().Be(searchItem.PostalCode);
        result.OrganisationId.Should().Be(searchItem.OrganisationId);
    }

    [Fact]
    public async Task ShouldReturnBasicData_WhenOrganisationIdIsProvided()
    {
        // given
        var organisationId = GuidTestData.GuidOne.ToString();

        OrganizationSearchServiceTestBuilder
            .New()
            .GetByOrganisationReturns(organisationId, null, out var searchItem)
            .Register(this);

        // when
        var result = await TestCandidate.Handle(new GetOrganisationQuery(organisationId), CancellationToken.None);

        // then
        result.OrganisationId.Should().Be(organisationId);
        result.CompaniesHouseNumber.Should().BeNull();
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
            .GetByOrganisationReturnsNoOrganization()
            .Register(this);

        // when
        var action = async () =>
            await TestCandidate.Handle(new GetOrganisationQuery(notExistingCompanyHouseNumber), CancellationToken.None);

        // then
        await action.Should().ThrowExactlyAsync<NotFoundException>().WithMessage($"Organization with id {notExistingCompanyHouseNumber} not found");
    }
}
