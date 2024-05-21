using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.DraftConsortiumEntityTests;

public class RemoveMemberTests
{
    [Fact]
    public void ShouldThrowException_WhenConfirmationIsNotProvided()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
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
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, false);

        // then
        testCandidate.Members.Should()
            .HaveCount(1)
            .And.Contain(new DraftConsortiumMember(
                InvestmentsOrganisationTestData.CactusDevelopments.Id,
                InvestmentsOrganisationTestData.CactusDevelopments.Name));
    }

    [Fact]
    public void ShouldThrowException_WhenMemberIsNotPartOfConsortium()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();

        // when
        var removeMember = () => testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, true);

        // then
        removeMember.Should().Throw<NotFoundException>();
    }

    [Fact]
    public void ShouldRemoveConsortiumMember_WhenConfirmationIsYes()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .Build();

        // when
        testCandidate.RemoveMember(InvestmentsOrganisationTestData.CactusDevelopments.Id, true);

        // then
        testCandidate.Members.Should().BeEmpty();
    }
}
