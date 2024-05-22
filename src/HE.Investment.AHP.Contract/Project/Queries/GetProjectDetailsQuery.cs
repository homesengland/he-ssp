using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Contract.Project.Queries;

public record GetProjectDetailsQuery(AhpProjectId ProjectId, PaginationRequest ApplicationPaginationRequest) : IRequest<ProjectDetailsModel>;
