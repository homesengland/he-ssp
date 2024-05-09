using FluentAssertions;
using HE.Investments.Account.Shared;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.QueryHandlers;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Tests.TestData;
using HE.Investments.TestsUtils.TestFramework;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.QueryHandlers;

public class GetConsortiumsListQueryHandlerTests : TestBase<GetConsortiumsListQueryHandler>
{
    [Fact]
    public async Task ShouldReturnConsortiumsList_WhenOrganisationBelongsToConsortiums()
    {
        // given
        var firstConsortiumId = new ConsortiumId("00000000-0000-1111-1111-111111111113");
        var firstConsortium = new ConsortiumEntityBuilder()
            .WithId(firstConsortiumId.Value)
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        var secondConsortiumId = new ConsortiumId("00000000-2222-3333-5555-111111111114");
        var secondConsortium = new ConsortiumEntityBuilder()
            .WithId(secondConsortiumId.Value)
            .WithLeadPartner(InvestmentsOrganisationTestData.CactusDevelopments)
            .WithActiveMember(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        var jjCompanyEmployee = UserAccountInvestmentsOrganisationTestData.JjCompanyEmployee;

        var consortiumsList = new List<ConsortiumEntity> { firstConsortium, secondConsortium, };

        var accountUserContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var consortiumRepository = CreateAndRegisterDependencyMock<IConsortiumRepository>();

        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(jjCompanyEmployee);
        consortiumRepository
            .Setup(x => x.GetConsortiumsListByMemberId(InvestmentsOrganisationTestData.JjCompany.Id, CancellationToken.None))
            .ReturnsAsync(consortiumsList);

        // when
        var result = await TestCandidate.Handle(new GetConsortiumsListQuery(), CancellationToken.None);

        // then
        result.Should().NotBeNull();
        result.OrganisationName.Should().Be(UserAccountTestData.AdminUserAccountOne.SelectedOrganisation().RegisteredCompanyName);
        result.Consortiums.Should().HaveCount(2);
        result.Consortiums.Should()
            .ContainEquivalentOf(new ConsortiumByMemberRole(
                    firstConsortiumId,
                    firstConsortium.Programme,
                    firstConsortium.LeadPartner.OrganisationName,
                    ConsortiumMembershipRole.LeadPartner));
        result.Consortiums.Should()
            .ContainEquivalentOf(new ConsortiumByMemberRole(
                secondConsortiumId,
                secondConsortium.Programme,
                secondConsortium.LeadPartner.OrganisationName,
                ConsortiumMembershipRole.Member));
    }

    [Fact]
    public async Task ShouldReturnEmptyConsortiumsList_WhenOrganisationDoesNotBelongToAnyConsortium()
    {
        // given
        var jjCompanyEmployee = UserAccountInvestmentsOrganisationTestData.JjCompanyEmployee;
        var accountUserContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var consortiumRepository = CreateAndRegisterDependencyMock<IConsortiumRepository>();

        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(jjCompanyEmployee);
        consortiumRepository
            .Setup(x => x.GetConsortiumsListByMemberId(InvestmentsOrganisationTestData.JjCompany.Id, CancellationToken.None))
            .ReturnsAsync([]);

        // when
        var result = await TestCandidate.Handle(new GetConsortiumsListQuery(), CancellationToken.None);

        // then
        result.Consortiums.Should().BeEmpty();
        result.OrganisationName.Should().Be(jjCompanyEmployee.Organisation!.RegisteredCompanyName);
    }
}
