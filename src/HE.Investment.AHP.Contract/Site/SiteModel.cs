using HE.Investment.AHP.Contract.Site.Enums;

namespace HE.Investment.AHP.Contract.Site;

public class SiteModel
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public Section106Dto? Section106 { get; set; }

    public SitePlanningDetails PlanningDetails { get; set; }

    public LocalAuthority? LocalAuthority { get; set; }

    public IList<NationalDesignGuidePriority> NationalDesignGuidePriorities { get; set; }

    public BuildingForHealthyLifeType BuildingForHealthyLife { get; set; }

    public string? NumberOfGreenLights { get; set; }

    public SiteLandAcquisitionStatus? LandAcquisitionStatus { get; set; }

    public SiteTenderingStatusDetails TenderingStatusDetails { get; set; }

    public StrategicSite StrategicSiteDetails { get; set; }

    public SiteTypeDetails SiteTypeDetails { get; set; }

    public SiteUseDetails SiteUseDetails { get; set; }

    public SiteRuralClassification RuralClassification { get; set; }

    public IList<SiteProcurement> SiteProcurements { get; set; }
}
