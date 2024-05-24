using FluentAssertions;
using HE.Investments.AHP.Consortium.Domain.Entities;
using HE.Investments.AHP.Consortium.Domain.Tests.TestData;
using HE.Investments.AHP.Consortium.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Consortium.Domain.Tests.Entities.DraftConsortiumEntityTests;

public class AddMemberTests
{
    [Fact]
    public async Task ShouldThrowException_WhenMemberIsLeadPartner()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        var addMember = () => testCandidate.AddMember(InvestmentsOrganisationTestData.JjCompany, isPartOfConsortium, CancellationToken.None);

        // then
        await addMember.Should()
            .ThrowAsync<DomainValidationException>()
            .WithMessage("The organisation you are trying to add is already added or being added as a member of this consortium");
    }

    [Fact]
    public async Task ShouldThrowException_WhenMemberIsAlreadyAddedToConsortium()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.CactusDevelopments)
            .WithMember(InvestmentsOrganisationTestData.MoralesEntertainment)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        var addMember = () => testCandidate.AddMember(InvestmentsOrganisationTestData.CactusDevelopments, isPartOfConsortium, CancellationToken.None);

        // then
        await addMember.Should()
            .ThrowAsync<DomainValidationException>()
            .WithMessage("The organisation you are trying to add is already added or being added as a member of this consortium");
    }

    [Fact]
    public async Task ShouldThrowException_WhenMemberIsAlreadyAddedToOtherConsortium()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .WithMember(InvestmentsOrganisationTestData.MoralesEntertainment)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsPartOfConsortium().Build();

        // when
        var addMember = () => testCandidate.AddMember(InvestmentsOrganisationTestData.CactusDevelopments, isPartOfConsortium, CancellationToken.None);

        // then
        await addMember.Should()
            .ThrowAsync<DomainValidationException>()
            .WithMessage("The organisation you are trying to add is already added or being added to another consortium");
    }

    [Fact]
    public async Task ShouldAddMember_WhenMemberIsNotAddedToAnyConsortium()
    {
        // given
        var testCandidate = new DraftConsortiumEntityBuilder()
            .WithLeadPartner(InvestmentsOrganisationTestData.JjCompany)
            .Build();
        var isPartOfConsortium = IsPartOfConsortiumBuilder.New().IsNotPartOfConsortium().Build();

        // when
        await testCandidate.AddMember(InvestmentsOrganisationTestData.CactusDevelopments, isPartOfConsortium, CancellationToken.None);

        // then
        testCandidate.Members.Should()
            .HaveCount(1)
            .And.Contain(
                new DraftConsortiumMember(
                    InvestmentsOrganisationTestData.CactusDevelopments.Id,
                    InvestmentsOrganisationTestData.CactusDevelopments.Name));
    }
}
