using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investment.AHP.Contract.PrefillData.Queries;

public record GetNewAhpSitePrefillDataQuery(FrontDoorProjectId ProjectId, FrontDoorSiteId SiteId) : IRequest<NewAhpSitePrefillData>;
