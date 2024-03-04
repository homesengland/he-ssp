using HE.Investments.FrontDoor.Contract.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Queries;

public record GetSiteDetailsQuery(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId) : IRequest<SiteDetails>;
