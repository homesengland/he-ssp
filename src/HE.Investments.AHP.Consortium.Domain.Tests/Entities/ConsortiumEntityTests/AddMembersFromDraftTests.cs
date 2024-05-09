using FluentAssertions;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.ConsortiumEntityTests;

public class AddMembersFromDraftTests
{
    [Fact]
    public void ShouldThrowException_WhenDraftConsortiumHasDifferentId()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var draftConsortium = new DraftConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-222222222222")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var addMembers = () => testCandidate.AddMembersFromDraft(draftConsortium, AreAllMembersAdded.Yes);

        // then
        addMembers.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenDraftConsortiumHasDifferentLeadPartner()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var draftConsortium = new DraftConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var addMembers = () => testCandidate.AddMembersFromDraft(draftConsortium, AreAllMembersAdded.Yes);

        // then
        addMembers.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldThrowException_WhenResponseIsNotProvided()
    {
        // given
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var draftConsortium = new DraftConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var addMembers = () => testCandidate.AddMembersFromDraft(draftConsortium, null);

        // then
        addMembers.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldNotAddMembers_WhenResponseIsNo()
    {
        // given
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var draftConsortium = new DraftConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var result = testCandidate.AddMembersFromDraft(draftConsortium, AreAllMembersAdded.No);

        // then
        result.Should().BeFalse();
        testCandidate.Members.Should().BeEmpty();
    }

    [Fact]
    public void ShouldAddMembers_WhenResponseIsYes()
    {
        // given
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var draftConsortium = new DraftConsortiumEntityBuilder()
            .WithId("00000000-0000-1111-1111-111111111111")
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        var result = testCandidate.AddMembersFromDraft(draftConsortium, AreAllMembersAdded.Yes);

        // then
        result.Should().BeTrue();
        var member = testCandidate.Members.Should().HaveCount(1).And.Subject.Single();

        member.Id.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        member.OrganisationName.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Name);
        member.Status.Should().Be(ConsortiumMemberStatus.PendingAddition);

        testCandidate.PopJoinRequest().Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        testCandidate.PopJoinRequest().Should().BeNull();
    }
}
