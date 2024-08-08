using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investments.AHP.ProjectDashboard.Contract.Project.Queries;

public record GetProjectsListQuery(PaginationRequest PaginationRequest) : IRequest<ProjectsListModel>;
