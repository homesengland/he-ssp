using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.FrontDoor.Contract.Site.Queries;

public record GetProjectSitesQuery(FrontDoorProjectId ProjectId) : IRequest<ProjectSites>;
