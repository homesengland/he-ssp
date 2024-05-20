using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Contract.Site;

public class SiteModel
{
    public string? Id { get; set; }

    public string Name { get; set; }

    public SiteStatus Status { get; set; }

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

    public string? EnvironmentalImpact { get; set; }

    public IList<SiteProcurement> SiteProcurements { get; set; }

    public SiteModernMethodsOfConstruction ModernMethodsOfConstruction { get; set; }

    public bool IsConsortiumMember { get; set; }

    public OrganisationDetails? DevelopingPartner { get; set; }

    public OrganisationDetails? OwnerOfTheLand { get; set; }

    public OrganisationDetails? OwnerOfTheHomes { get; set; }

    public bool IsReadOnly => Status == SiteStatus.Completed;
}
