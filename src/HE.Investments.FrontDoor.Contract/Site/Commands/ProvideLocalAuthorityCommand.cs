using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.FrontDoor.Contract.Site.Commands;

public record ProvideLocalAuthorityCommand(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId, LocalAuthorityId LocalAuthorityId) : IProvideSiteDetailsCommand;
