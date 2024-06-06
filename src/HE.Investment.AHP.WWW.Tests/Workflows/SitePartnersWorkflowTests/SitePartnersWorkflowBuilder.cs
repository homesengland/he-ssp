using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investment.AHP.WWW.Workflows;

namespace HE.Investment.AHP.WWW.Tests.Workflows.SitePartnersWorkflowTests;

internal sealed class SitePartnersWorkflowBuilder
{
    private readonly SitePartnersWorkflowState _state;

    private readonly SiteModel _site = new();

    public SitePartnersWorkflowBuilder(SitePartnersWorkflowState state = SitePartnersWorkflowState.FlowStarted)
    {
        _state = state;
    }

    public SitePartnersWorkflowBuilder WithIsConsortiumMember(bool isConsortiumMember = true)
    {
        _site.IsConsortiumMember = isConsortiumMember;
        return this;
    }

    public SitePartnersWorkflowBuilder WithIsUnregisteredBody(bool isUnregisteredBody = true)
    {
        _site.IsUnregisteredBody = isUnregisteredBody;
        return this;
    }

    public SitePartnersWorkflow Build()
    {
        return new SitePartnersWorkflow(_state, _site);
    }
}
