using FluentAssertions;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.ConsortiumEntityTests;

public class RemoveMemberTests
{
    [Fact]
    public async Task ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var consortiumPartnerStatusProvider = MockConsortiumPartnerStatusProvider(ConsortiumPartnerStatus.None);
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            null,
            consortiumPartnerStatusProvider,
            CancellationToken.None);

        // then
        await removeMember.Should().ThrowAsync<DomainValidationException>().WithMessage("Select yes if you want to remove the organisation");
    }

    [Fact]
    public async Task ShouldDoNothing_WhenConfirmationIsNo()
    {
        // given
        var consortiumPartnerStatusProvider = MockConsortiumPartnerStatusProvider(ConsortiumPartnerStatus.SitePartner);
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        await testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            false,
            consortiumPartnerStatusProvider,
            CancellationToken.None);

        // then
        testCandidate.Members.Should()
            .HaveCount(1)
            .And.Contain(new ConsortiumMember(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                InvestmentsOrganisationTestData.CactusDevelopments.Name,
                ConsortiumMemberStatus.Active));
        testCandidate.ActiveMembers.Should().HaveCount(1);
        testCandidate.PopRemoveRequest().Should().BeNull();
    }

    [Fact]
    public async Task ShouldThrowException_WhenMemberIsNotPartOfConsortium()
    {
        // given
        var consortiumPartnerStatusProvider = MockConsortiumPartnerStatusProvider(ConsortiumPartnerStatus.None);
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            true,
            consortiumPartnerStatusProvider,
            CancellationToken.None);

        // then
        await removeMember.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData(ConsortiumPartnerStatus.SitePartner, "This organisation is a Site Partner")]
    [InlineData(ConsortiumPartnerStatus.ApplicationPartner, "This organisation is an Application Partner")]
    public async Task ShouldThrowException_WhenConfirmationIsYesAndOrganisationIsPartner(ConsortiumPartnerStatus status, string expectedMessage)
    {
        // given
        var consortiumPartnerStatusProvider = MockConsortiumPartnerStatusProvider(status);
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            true,
            consortiumPartnerStatusProvider,
            CancellationToken.None);

        // then
        await removeMember.Should().ThrowAsync<DomainValidationException>().WithMessage(expectedMessage);
    }

    [Fact]
    public async Task ShouldMarkMemberAsPendingRemoval_WhenConfirmationIsYesAndOrganisationIsNotPartner()
    {
        // given
        var consortiumPartnerStatusProvider = MockConsortiumPartnerStatusProvider(ConsortiumPartnerStatus.None);
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        await testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            true,
            consortiumPartnerStatusProvider,
            CancellationToken.None);

        // then
        testCandidate.Members.Should()
            .HaveCount(1)
            .And.Contain(new ConsortiumMember(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                InvestmentsOrganisationTestData.CactusDevelopments.Name,
                ConsortiumMemberStatus.PendingRemoval));
        testCandidate.ActiveMembers.Should().BeEmpty();
        testCandidate.PopRemoveRequest().Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
    }

    private IConsortiumPartnerStatusProvider MockConsortiumPartnerStatusProvider(ConsortiumPartnerStatus status)
    {
        var provider = new Mock<IConsortiumPartnerStatusProvider>();
        provider.Setup(x => x.GetConsortiumPartnerStatus(It.IsAny<ConsortiumId>(), It.IsAny<OrganisationId>(), CancellationToken.None))
            .ReturnsAsync(status);

        return provider.Object;
    }
}
