using HE.Investment.AHP.Contract.Site.Enums;

namespace HE.Investment.AHP.Contract.Site;

public class SiteModel
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public bool? Section106GeneralAgreement { get; set; }

    public bool? Section106AffordableHousing { get; set; }

    public bool? Section106OnlyAffordableHousing { get; set; }

    public bool? Section106AdditionalAffordableHousing { get; set; }

    public bool? Section106CapitalFundingEligibility { get; set; }

    public string? Section106LocalAuthorityConfirmation { get; set; }

    public bool? IsIneligible { get; set; }

    public bool? IsIneligibleDueToCapitalFundingGuide { get; set; }

    public bool? IsIneligibleDueToAffordableHousing { get; set; }

    public SitePlanningDetails PlanningDetails { get; set; }

    public LocalAuthority? LocalAuthority { get; set; }

    public IList<NationalDesignGuidePriority> NationalDesignGuidePriorities { get; set; }

    public SiteTenderingStatusDetails TenderingStatusDetails { get; set; }

    public StrategicSite StrategicSiteDetails { get; set; }
}
