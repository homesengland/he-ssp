using HE.Investment.AHP.Contract.Site.Enums;

namespace HE.Investment.AHP.Contract.Site;

public record SiteUseDetails(bool? IsPartOfStreetFrontInfill, bool? IsForTravellerPitchSite, TravellerPitchSiteType TravellerPitchSiteType);
