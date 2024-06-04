using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Moq;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.DraftConsortiumEntityTests;

public class RemoveMemberTests
{
    [Fact]
    public async Task ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            null,
            It.IsAny<IConsortiumPartnerStatusProvider>(),
            CancellationToken.None);

        // then
        await removeMember.Should().ThrowAsync<DomainValidationException>().WithMessage("Select yes if you want to remove the organisation");
    }

    [Fact]
    public async Task ShouldDoNothing_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        await testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            false,
            It.IsAny<IConsortiumPartnerStatusProvider>(),
            CancellationToken.None);

        // then
        testCandidate.Members.Should()
            .HaveCount(1)
            .And.Contain(new DraftConsortiumMember(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                InvestmentsOrganisationTestData.CactusDevelopments.Name));
    }

    [Fact]
    public async Task ShouldThrowException_WhenMemberIsNotPartOfConsortium()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            true,
            It.IsAny<IConsortiumPartnerStatusProvider>(),
            CancellationToken.None);

        // then
        await removeMember.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ShouldRemoveConsortiumMember_WhenConfirmationIsYes()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        await testCandidate.RemoveMember(
            InvestmentsOrganisationTestData.CactusDevelopments.Id,
            true,
            It.IsAny<IConsortiumPartnerStatusProvider>(),
            CancellationToken.None);

        // then
        testCandidate.Members.Should().BeEmpty();
    }
}
