using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Organisation.Contract;

namespace HE.Investment.AHP.WWW.Workflows;

public class SitePartnersWorkflow : EncodedStateRouting<SitePartnersWorkflowState>
{
    private readonly SiteModel? _siteModel;

    public SitePartnersWorkflow(SitePartnersWorkflowState currentWorkflowState, SiteModel? siteModel, bool isLocked = false)
        : base(currentWorkflowState, isLocked)
    {
        _siteModel = siteModel;
        ConfigureTransitions();
    }

    public override bool CanBeAccessed(SitePartnersWorkflowState state, bool? isReadOnlyMode = null)
    {
        return state switch
        {
            SitePartnersWorkflowState.FlowStarted => true,
            SitePartnersWorkflowState.DevelopingPartner => IsConsortiumMember(),
            SitePartnersWorkflowState.DevelopingPartnerConfirm => IsConsortiumMember(),
            SitePartnersWorkflowState.OwnerOfTheLand => IsConsortiumMember(),
            SitePartnersWorkflowState.OwnerOfTheLandConfirm => IsConsortiumMember(),
            SitePartnersWorkflowState.OwnerOfTheHomes => IsConsortiumMember(),
            SitePartnersWorkflowState.OwnerOfTheHomesConfirm => IsConsortiumMember(),
            SitePartnersWorkflowState.UnregisteredBodySearch => IsUnregisteredBody(),
            SitePartnersWorkflowState.UnregisteredBodySearchResult => IsUnregisteredBody(),
            SitePartnersWorkflowState.UnregisteredBodySearchNoResults => IsUnregisteredBody(),
            SitePartnersWorkflowState.UnregisteredBodyCreateManual => IsUnregisteredBody(),
            SitePartnersWorkflowState.UnregisteredBodyConfirm => IsUnregisteredBody(),
            SitePartnersWorkflowState.FlowFinished => true,
            _ => false,
        };
    }

    private void ConfigureTransitions()
    {
        Machine.Configure(SitePartnersWorkflowState.FlowStarted)
            .PermitIf(Trigger.Continue, SitePartnersWorkflowState.DevelopingPartner, IsConsortiumMember)
            .PermitIf(Trigger.Continue, SitePartnersWorkflowState.UnregisteredBodySearch, IsUnregisteredBody)
            .PermitIf(Trigger.Continue, SitePartnersWorkflowState.FlowFinished, () => !IsConsortiumMember() && !IsUnregisteredBody());

        Machine.Configure(SitePartnersWorkflowState.DevelopingPartner)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.DevelopingPartnerConfirm)
            .Permit(Trigger.Back, SitePartnersWorkflowState.FlowStarted);

        Machine.Configure(SitePartnersWorkflowState.DevelopingPartnerConfirm)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.OwnerOfTheLand)
            .Permit(Trigger.Back, SitePartnersWorkflowState.DevelopingPartner);

        Machine.Configure(SitePartnersWorkflowState.OwnerOfTheLand)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.OwnerOfTheLandConfirm)
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.DevelopingPartner, () => !IsPartnerProvided(x => x.DevelopingPartner))
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.DevelopingPartnerConfirm, () => IsPartnerProvided(x => x.DevelopingPartner));

        Machine.Configure(SitePartnersWorkflowState.OwnerOfTheLandConfirm)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.OwnerOfTheHomes)
            .Permit(Trigger.Back, SitePartnersWorkflowState.OwnerOfTheLand);

        Machine.Configure(SitePartnersWorkflowState.OwnerOfTheHomes)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.OwnerOfTheHomesConfirm)
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.OwnerOfTheLand, () => !IsPartnerProvided(x => x.OwnerOfTheLand))
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.OwnerOfTheLandConfirm, () => IsPartnerProvided(x => x.OwnerOfTheLand));

        Machine.Configure(SitePartnersWorkflowState.OwnerOfTheHomesConfirm)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.FlowFinished)
            .Permit(Trigger.Back, SitePartnersWorkflowState.OwnerOfTheHomes);

        Machine.Configure(SitePartnersWorkflowState.UnregisteredBodySearch)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.UnregisteredBodySearchResult)
            .Permit(Trigger.Back, SitePartnersWorkflowState.FlowStarted);

        Machine.Configure(SitePartnersWorkflowState.UnregisteredBodySearchResult)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.UnregisteredBodyConfirm)
            .Permit(Trigger.Back, SitePartnersWorkflowState.UnregisteredBodySearch);

        Machine.Configure(SitePartnersWorkflowState.UnregisteredBodySearchNoResults)
            .Permit(Trigger.Back, SitePartnersWorkflowState.UnregisteredBodySearch);

        Machine.Configure(SitePartnersWorkflowState.UnregisteredBodyCreateManual)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.UnregisteredBodyConfirm)
            .Permit(Trigger.Back, SitePartnersWorkflowState.UnregisteredBodySearch);

        Machine.Configure(SitePartnersWorkflowState.UnregisteredBodyConfirm)
            .Permit(Trigger.Continue, SitePartnersWorkflowState.FlowFinished)
            .Permit(Trigger.Back, SitePartnersWorkflowState.UnregisteredBodySearch);

        Machine.Configure(SitePartnersWorkflowState.FlowFinished)
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.FlowStarted, () => !IsConsortiumMember() && !IsUnregisteredBody())
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.OwnerOfTheHomes, () => IsConsortiumMember() && !IsPartnerProvided(x => x.OwnerOfTheHomes))
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.OwnerOfTheHomesConfirm, () => IsConsortiumMember() && IsPartnerProvided(x => x.OwnerOfTheHomes))
            .PermitIf(Trigger.Back, SitePartnersWorkflowState.UnregisteredBodyConfirm, IsUnregisteredBody);
    }

    private bool IsPartnerProvided(Func<SiteModel, OrganisationDetails?> getPartner) => _siteModel != null && getPartner(_siteModel) != null;

    private bool IsConsortiumMember() => _siteModel?.IsConsortiumMember == true;

    [SuppressMessage("Maintainability", "S1125", Justification = "Nullable bool needs to be compared with boolean")]
    private bool IsUnregisteredBody() => _siteModel?.IsUnregisteredBody == true && !IsConsortiumMember();
}
