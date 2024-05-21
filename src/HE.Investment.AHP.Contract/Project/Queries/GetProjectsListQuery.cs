using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Contract.Project.Queries;

public record GetProjectsListQuery(PaginationRequest PaginationRequest) : IRequest<ProjectsListModel>;
