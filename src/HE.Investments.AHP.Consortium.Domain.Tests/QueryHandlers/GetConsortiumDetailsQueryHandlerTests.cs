extern alias Org;

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
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.Services;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.QueryHandlers;

public class GetConsortiumDetailsQueryHandlerTests : TestBase<GetConsortiumDetailsQueryHandler>
{
    [Fact]
    public async Task ShouldReturnConsortiumWithoutAddress_WhenDraftConsortiumExistsAndFetchAddressFlagIsFalse()
    {
        // given
        var consortiumId = new ConsortiumId("00000000-0000-1111-1111-111111111113");
        var consortium = new DraftConsortiumEntityBuilder()
            .WithId(consortiumId.Value)
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        var accountUserContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var draftRepository = CreateAndRegisterDependencyMock<IDraftConsortiumRepository>();

        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(UserAccountTestData.AdminUserAccountOne);
        draftRepository.Setup(x => x.Get(consortiumId, UserAccountTestData.AdminUserAccountOne, false)).Returns(consortium);

        // when
        var result = await TestCandidate.Handle(new GetConsortiumDetailsQuery(consortiumId, FetchAddress: false), CancellationToken.None);

        // then
        result.ConsortiumId.Should().Be(consortiumId);
        result.Programme.Should().Be(consortium.Programme);
        result.IsDraft.Should().BeTrue();
        result.LeadPartner.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.JjCompany.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.JjCompany.Name,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    InvestmentsOrganisationTestData.JjCompany.Id.Value)));

        var member = result.Members.Should().HaveCount(1).And.Subject.Single();
        member.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.CactusDevelopments.Name,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    InvestmentsOrganisationTestData.CactusDevelopments.Id.Value)));
    }

    [Fact]
    public async Task ShouldReturnConsortiumWithAddress_WhenDraftConsortiumExistsAndFetchAddressFlagIsTrue()
    {
        // given
        var consortiumId = new ConsortiumId("00000000-0000-1111-1111-111111111113");
        var consortium = new DraftConsortiumEntityBuilder()
            .WithId(consortiumId.Value)
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();
        var organisations = new[]
        {
            new OrganizationDetailsDto
            {
                organisationId = InvestmentsOrganisationTestData.JjCompany.Id.Value,
                registeredCompanyName = InvestmentsOrganisationTestData.JjCompany.Name,
                addressLine1 = "Street 1",
                city = "City 1",
                postalcode = "12345",
                companyRegistrationNumber = "AB1234",
            },
            new OrganizationDetailsDto
            {
                organisationId = InvestmentsOrganisationTestData.CactusDevelopments.Id.Value,
                registeredCompanyName = InvestmentsOrganisationTestData.CactusDevelopments.Name,
                addressLine1 = "Street 2",
                city = "City 2",
                postalcode = "23456",
                companyRegistrationNumber = "AB2345",
            },
        };

        var accountUserContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var draftRepository = CreateAndRegisterDependencyMock<IDraftConsortiumRepository>();
        var organisationSearchService = CreateAndRegisterDependencyMock<IOrganizationCrmSearchService>();

        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(UserAccountTestData.AdminUserAccountOne);
        draftRepository.Setup(x => x.Get(consortiumId, UserAccountTestData.AdminUserAccountOne, false)).Returns(consortium);
        organisationSearchService.Setup(x =>
                x.GetOrganizationFromCrmByOrganisationId(new[]
                {
                    InvestmentsOrganisationTestData.JjCompany.Id.Value, InvestmentsOrganisationTestData.CactusDevelopments.Id.Value,
                }))
            .ReturnsAsync(organisations);

        // when
        var result = await TestCandidate.Handle(new GetConsortiumDetailsQuery(consortiumId, FetchAddress: true), CancellationToken.None);

        // then
        result.ConsortiumId.Should().Be(consortiumId);
        result.Programme.Should().Be(consortium.Programme);
        result.IsDraft.Should().BeTrue();
        result.LeadPartner.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.JjCompany.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.JjCompany.Name,
                    "Street 1",
                    "City 1",
                    "12345",
                    "AB1234",
                    InvestmentsOrganisationTestData.JjCompany.Id.Value)));

        var member = result.Members.Should().HaveCount(1).And.Subject.Single();
        member.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.CactusDevelopments.Name,
                    "Street 2",
                    "City 2",
                    "23456",
                    "AB2345",
                    InvestmentsOrganisationTestData.CactusDevelopments.Id.Value)));
    }

    [Fact]
    public async Task ShouldReturnConsortiumWithoutAddress_WhenDraftConsortiumDoesNotExistAndFetchAddressFlagIsFalse()
    {
        // given
        var consortiumId = new ConsortiumId("00000000-0000-1111-1111-111111111113");
        var consortium = new ConsortiumEntityBuilder()
            .WithId(consortiumId.Value)
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        var accountUserContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var repository = CreateAndRegisterDependencyMock<IConsortiumRepository>();
        var draftRepository = CreateAndRegisterDependencyMock<IDraftConsortiumRepository>();

        draftRepository.Setup(x => x.Get(consortiumId, UserAccountTestData.AdminUserAccountOne, false)).Returns((DraftConsortiumEntity?)null);
        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(UserAccountTestData.AdminUserAccountOne);
        repository.Setup(x => x.GetConsortium(consortiumId, UserAccountTestData.AdminUserAccountOne, CancellationToken.None)).ReturnsAsync(consortium);

        // when
        var result = await TestCandidate.Handle(new GetConsortiumDetailsQuery(consortiumId, FetchAddress: false), CancellationToken.None);

        // then
        result.ConsortiumId.Should().Be(consortiumId);
        result.Programme.Should().Be(consortium.Programme);
        result.IsDraft.Should().BeFalse();
        result.LeadPartner.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.JjCompany.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.JjCompany.Name,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    InvestmentsOrganisationTestData.JjCompany.Id.Value)));

        var member = result.Members.Should().HaveCount(1).And.Subject.Single();
        member.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.CactusDevelopments.Name,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    InvestmentsOrganisationTestData.CactusDevelopments.Id.Value)));
    }

    [Fact]
    public async Task ShouldReturnConsortiumWithAddress_WhenDraftConsortiumDoesNotExistAndFetchAddressFlagIsTrue()
    {
        // given
        var consortiumId = new ConsortiumId("00000000-0000-1111-1111-111111111113");
        var consortium = new ConsortiumEntityBuilder()
            .WithId(consortiumId.Value)
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();
        var organisations = new[]
        {
            new OrganizationDetailsDto
            {
                organisationId = InvestmentsOrganisationTestData.JjCompany.Id.Value,
                registeredCompanyName = InvestmentsOrganisationTestData.JjCompany.Name,
                addressLine1 = "Street 1",
                city = "City 1",
                postalcode = "12345",
                companyRegistrationNumber = "AB1234",
            },
            new OrganizationDetailsDto
            {
                organisationId = InvestmentsOrganisationTestData.CactusDevelopments.Id.Value,
                registeredCompanyName = InvestmentsOrganisationTestData.CactusDevelopments.Name,
                addressLine1 = "Street 2",
                city = "City 2",
                postalcode = "23456",
                companyRegistrationNumber = "AB2345",
            },
        };

        var accountUserContext = CreateAndRegisterDependencyMock<IAccountUserContext>();
        var repository = CreateAndRegisterDependencyMock<IConsortiumRepository>();
        var organisationSearchService = CreateAndRegisterDependencyMock<IOrganizationCrmSearchService>();
        var draftRepository = CreateAndRegisterDependencyMock<IDraftConsortiumRepository>();

        accountUserContext.Setup(x => x.GetSelectedAccount()).ReturnsAsync(UserAccountTestData.AdminUserAccountOne);
        draftRepository.Setup(x => x.Get(consortiumId, UserAccountTestData.AdminUserAccountOne, false)).Returns((DraftConsortiumEntity?)null);
        repository.Setup(x => x.GetConsortium(consortiumId, UserAccountTestData.AdminUserAccountOne, CancellationToken.None)).ReturnsAsync(consortium);
        organisationSearchService.Setup(x =>
                x.GetOrganizationFromCrmByOrganisationId(new[]
                {
                    InvestmentsOrganisationTestData.JjCompany.Id.Value, InvestmentsOrganisationTestData.CactusDevelopments.Id.Value,
                }))
            .ReturnsAsync(organisations);

        // when
        var result = await TestCandidate.Handle(new GetConsortiumDetailsQuery(consortiumId, FetchAddress: true), CancellationToken.None);

        // then
        result.ConsortiumId.Should().Be(consortiumId);
        result.Programme.Should().Be(consortium.Programme);
        result.IsDraft.Should().BeFalse();
        result.LeadPartner.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.JjCompany.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.JjCompany.Name,
                    "Street 1",
                    "City 1",
                    "12345",
                    "AB1234",
                    InvestmentsOrganisationTestData.JjCompany.Id.Value)));

        var member = result.Members.Should().HaveCount(1).And.Subject.Single();
        member.Should()
            .Be(new ConsortiumMemberDetails(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                ConsortiumMemberStatus.Active,
                new OrganisationDetails(
                    InvestmentsOrganisationTestData.CactusDevelopments.Name,
                    "Street 2",
                    "City 2",
                    "23456",
                    "AB2345",
                    InvestmentsOrganisationTestData.CactusDevelopments.Id.Value)));
    }
}
