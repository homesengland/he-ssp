using FluentAssertions;
using HE.Investment.AHP.Contract.SitePartners;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SitePartnersWorkflowTests;

public class CanBeAccessedTests
{
    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowStarted, true)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartner, true)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartnerConfirm, true)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLand, true)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLandConfirm, true)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomes, true)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomesConfirm, true)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearch, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchResult, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchNoResults, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyCreateManual, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyConfirm, false)]
    [InlineData(SitePartnersWorkflowState.FlowFinished, true)]
    public async Task ShouldReturnStateAccessibility_WhenOrganisationIsConsortiumMember(SitePartnersWorkflowState state, bool canBeAccessed)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder().WithIsConsortiumMember().Build();

        // when
        var result = await testCandidate.StateCanBeAccessed(state);

        // then
        result.Should().Be(canBeAccessed);
    }

    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowStarted, true)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartner, false)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartnerConfirm, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLand, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLandConfirm, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomes, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomesConfirm, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearch, true)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchResult, true)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchNoResults, true)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyCreateManual, true)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyConfirm, true)]
    [InlineData(SitePartnersWorkflowState.FlowFinished, true)]
    public async Task ShouldReturnStateAccessibility_WhenOrganisationIsUnregisteredBody(SitePartnersWorkflowState state, bool canBeAccessed)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder().WithIsUnregisteredBody().Build();

        // when
        var result = await testCandidate.StateCanBeAccessed(state);

        // then
        result.Should().Be(canBeAccessed);
    }

    [Theory]
    [InlineData(SitePartnersWorkflowState.FlowStarted, true)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartner, false)]
    [InlineData(SitePartnersWorkflowState.DevelopingPartnerConfirm, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLand, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheLandConfirm, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomes, false)]
    [InlineData(SitePartnersWorkflowState.OwnerOfTheHomesConfirm, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearch, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchResult, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodySearchNoResults, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyCreateManual, false)]
    [InlineData(SitePartnersWorkflowState.UnregisteredBodyConfirm, false)]
    [InlineData(SitePartnersWorkflowState.FlowFinished, true)]
    public async Task ShouldReturnStateAccessibility_WhenOrganisationNotUnregisteredBodyAndNotInConsortium(SitePartnersWorkflowState state, bool canBeAccessed)
    {
        // given
        var testCandidate = new SitePartnersWorkflowBuilder()
            .WithIsConsortiumMember(false)
            .WithIsUnregisteredBody(false)
            .Build();

        // when
        var result = await testCandidate.StateCanBeAccessed(state);

        // then
        result.Should().Be(canBeAccessed);
    }
}
