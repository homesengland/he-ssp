using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;

namespace HE.Investment.AHP.Domain.Common;

public record SiteBasicInfo(SiteId Id, SiteName Name, SiteUsingModernMethodsOfConstruction SiteUsingModernMethodsOfConstruction);
