namespace HE.Investment.AHP.Contract.Site;

public record Section106Dto(
    string SiteId,
    string SiteName,
    bool? GeneralAgreement,
    bool? AffordableHousing = null,
    bool? OnlyAffordableHousing = null,
    bool? AdditionalAffordableHousing = null,
    bool? CapitalFundingEligibility = null,
    string? LocalAuthorityConfirmation = null,
    bool? IsIneligible = false,
    bool? IsIneligibleDueToAffordableHousing = false,
    bool? IsIneligibleDueToCapitalFunding = false);
