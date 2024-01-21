namespace HE.Investment.AHP.Contract.Site;

public enum SiteWorkflowState
{
    Index = 1,
    Start,
    Name,
    Section106GeneralAgreement,
    Section106AffordableHousing,
    Section106OnlyAffordableHousing,
    Section106AdditionalAffordableHousing,
    Section106CapitalFundingEligibility,
    Section106ConfirmationFromLocalAuthority,
    Ineligable,
    LocalAuthority,
}
