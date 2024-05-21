using FluentAssertions;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.AHP.Consortium.Domain.ValueObjects;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.ConsortiumEntityTests;

public class RemoveMemberTests
{
    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, null);

        // then
        removeMember.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldDoNothing_WhenConfirmationIsNo()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, false);

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
    public void ShouldThrowException_WhenMemberIsNotPartOfConsortium()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, true);

        // then
        removeMember.Should().Throw<NotFoundException>();
    }

    [Fact]
    public void ShouldMarkMemberAsPendingRemoval_WhenConfirmationIsYes()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, true);

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
}
