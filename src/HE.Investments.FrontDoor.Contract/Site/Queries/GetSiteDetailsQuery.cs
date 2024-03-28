using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Queries;

public record GetSiteDetailsQuery(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId) : IRequest<SiteDetails>;
