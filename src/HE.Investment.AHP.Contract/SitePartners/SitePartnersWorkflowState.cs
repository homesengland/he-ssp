namespace HE.Investment.AHP.Contract.SitePartners;

public enum SitePartnersWorkflowState
{
    FlowStarted,
    DevelopingPartner,
    DevelopingPartnerConfirm,
    OwnerOfTheLand,
    OwnerOfTheLandConfirm,
    OwnerOfTheHomes,
    OwnerOfTheHomesConfirm,
    UnregisteredBodySearch,
    UnregisteredBodySearchResult,
    UnregisteredBodySearchNoResults,
    UnregisteredBodyCreateManual,
    UnregisteredBodyConfirm,
    FlowFinished,
}
