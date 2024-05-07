extern alias Org;

using FluentAssertions;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.ConsortiumEntityTests;

public class AddMemberTests
{
    [Fact]
    public async Task ShouldThrowException_WhenMemberIsLeadPartner()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        var addMember = () => testCandidate.AddMember(InvestmentsOrganisationTestData.JjCompany, isPartOfConsortium, CancellationToken.None);

        // then
        await addMember.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenMemberIsAlreadyAddedToConsortium()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .WithActiveMember(InvestmentsOrganisationTestData.MoralesEntertainment)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        var addMember = () => testCandidate.AddMember(InvestmentsOrganisationTestData.CactusDevelopments, isPartOfConsortium, CancellationToken.None);

        // then
        await addMember.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldThrowException_WhenMemberIsAlreadyAddedToOtherConsortium()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithActiveMember(InvestmentsOrganisationTestData.MoralesEntertainment)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsPartOfConsortium().Build();

        // when
        var addMember = () => testCandidate.AddMember(InvestmentsOrganisationTestData.CactusDevelopments, isPartOfConsortium, CancellationToken.None);

        // then
        await addMember.Should().ThrowAsync<DomainValidationException>();
    }

    [Fact]
    public async Task ShouldAddPendingMember_WhenMemberIsNotAddedToAnyConsortium()
    {
        // given
        var testCandidate = new ConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        await testCandidate.AddMember(InvestmentsOrganisationTestData.CactusDevelopments, isPartOfConsortium, CancellationToken.None);

        // then
        var member = testCandidate.Members.Should().HaveCount(1).And.Subject.Single();

        member.Id.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        member.OrganisationName.Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Name);
        member.Status.Should().Be(ConsortiumMemberStatus.PendingAddition);

        testCandidate.PopJoinRequest().Should().Be(InvestmentsOrganisationTestData.CactusDevelopments.Id);
        testCandidate.PopJoinRequest().Should().BeNull();
    }
}
