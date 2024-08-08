using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project.Queries;

public record GetProjectDetailsQuery(FrontDoorProjectId ProjectId, PaginationRequest ApplicationPaginationRequest) : IRequest<ProjectDetailsModel>;
